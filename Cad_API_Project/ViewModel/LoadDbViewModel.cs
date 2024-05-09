using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Cad_API_Project.DataContext;
using System;
using System.Data.SqlClient;
using System.Data;
using Cad_API_Project.Command;

namespace Cad_API_Project.ViewModel
{
    public class LoadDbViewModel
    {
        // Load all the Line Objects into Database
        #region Load Lines

        public string LoadLines()
        {
            string result = "";
            SqlConnection conn = DbUtil.GetConnection();

            try
            {
                // Get the Document and Editor object
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);
                    SelectionFilter filter = new SelectionFilter(tv);

                    PromptSelectionResult ssPrompt = ed.SelectAll(filter);
                    // Check if there is object selected
                    if (ssPrompt.Status == PromptStatus.OK)
                    {
                        double startPtX = 0.0, startPtY = 0.0, endPtX = 0.0, endPtY = 0.0;
                        string layer = "", ltype = "", color = "";
                        double len = 0.0;
                        Line line = new Line();
                        SelectionSet ss = ssPrompt.Value;

                        //sql = "INSERT INTO dbo.Lines (StartPtX, StartPtY, EndPtX, EndPtY, Layer, Color, Linetype, Length) VALUES (" + startPtX + "," + startPtY + "," + endPtX + "," + endPtY + ",'" + layer + "','" + color + "','" + ltype + "'," + len + ")";
                        String sql = @"INSERT INTO dbo.Lines (StartPtX, StartPtY, EndPtX, EndPtY, Layer, Color, Linetype, Length, Created) 
                                       VALUES(@StartPtX, @StartPtY, @EndPtX, @EndPtY, @Layer, @Color, @Linetype, @Length, @Created)";
                        conn.Open();

                        // Loop through the selection set and insert into database one line object at a time
                        foreach (SelectedObject sObj in ss)
                        {
                            line = trans.GetObject(sObj.ObjectId, OpenMode.ForRead) as Line;
                            startPtX = line.StartPoint.X;
                            startPtY = line.StartPoint.Y;
                            endPtX = line.EndPoint.X;
                            endPtY = line.EndPoint.Y;
                            layer = line.Layer;
                            ltype = line.Linetype;
                            color = line.Color.ToString();
                            len = line.Length;

                            SqlCommand cmd = new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@StartPtX", startPtX);
                            cmd.Parameters.AddWithValue("@StartPtY", startPtY);
                            cmd.Parameters.AddWithValue("@EndPtX", endPtX);
                            cmd.Parameters.AddWithValue("@EndPtY", endPtY);
                            cmd.Parameters.AddWithValue("@Layer", layer);
                            cmd.Parameters.AddWithValue("@Color", color);
                            cmd.Parameters.AddWithValue("@Linetype", ltype);
                            cmd.Parameters.AddWithValue("@Length", len);
                            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        ed.WriteMessage("No object selected.");
                    }
                    trans.Commit();

                    result = "Done. Completed successfully!";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }

        #endregion



        // Load all the MText Objects into Database
        #region Load MTexts

        public string LoadMTexts()
        {
            string result = "";
            SqlConnection conn = DbUtil.GetConnection();
            try
            {
                // Get the Document and Editor object
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                Editor ed = doc.Editor;

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, "MTEXT"), 0);
                    SelectionFilter filter = new SelectionFilter(tv);

                    PromptSelectionResult ssPrompt = ed.SelectAll(filter);
                    // Check if there is object selected
                    if (ssPrompt.Status == PromptStatus.OK)
                    {
                        double insPtX = 0.0, insPtY = 0.0;
                        string layer = "", textstyle = "";
                        string color = "";
                        double height = 0.0, width = 0.0;
                        int attachment;
                        MText mtx = new MText();
                        string tx = "";
                        SelectionSet ss = ssPrompt.Value;

                        //sql = "INSERT INTO dbo.MTexts (insPtX, insPtY, Layer, Color, TextStyle, Height, Width, Text) VALUES (" + insPtX + "," + insPtX + "," + layer + "," + color + ",'" + textstyle + "','" + height + "','" + width + ",'" + tx + "')";
                        string sql = @"INSERT INTO dbo.MTexts (insPtX, insPtY, Layer, Color, TextStyle, Height, Width, Text, Attachment, Created) 
                                    VALUES(@InsPtX, @InsPtY, @Layer, @Color, @TextStyle, @Height, @Width, @Text, @Attachment, @Created)";
                        conn.Open();

                        // Loop through the selection set and insert into database one mtext object at a time
                        foreach (SelectedObject sObj in ss)
                        {
                            mtx = trans.GetObject(sObj.ObjectId, OpenMode.ForRead) as MText;

                            insPtX = mtx.Location.X;
                            insPtY = mtx.Location.Y;
                            layer = mtx.Layer;
                            color = mtx.Color.ToString();
                            textstyle = mtx.TextStyleName;
                            height = mtx.TextHeight;
                            width = mtx.Width;
                            tx = mtx.Contents;
                            attachment = Convert.ToInt32(mtx.Attachment);

                            SqlCommand cmd = new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@InsPtX", insPtX);
                            cmd.Parameters.AddWithValue("@InsPtY", insPtY);
                            cmd.Parameters.AddWithValue("@Layer", layer);
                            cmd.Parameters.AddWithValue("@Color", color);
                            cmd.Parameters.AddWithValue("@TextStyle", textstyle);
                            cmd.Parameters.AddWithValue("@Height", height);
                            cmd.Parameters.AddWithValue("@Width", width);
                            cmd.Parameters.AddWithValue("@Text", tx);
                            cmd.Parameters.AddWithValue("@Attachment", attachment);
                            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        ed.WriteMessage("No object selected.");
                    }
                    trans.Commit();

                }
                result = "Done. Completed successfully!";
            }
            catch (System.Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }

        #endregion



        // Load all the Polyline Objects
        #region Load Polylines

        public string LoadPolylines()
        {
            string result = "";
            SqlConnection conn = DbUtil.GetConnection();
            try
            {
                // Get the Document and Editor object
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, "LWPOLYLINE"), 0);
                    SelectionFilter filter = new SelectionFilter(tv);

                    PromptSelectionResult ssPrompt = ed.SelectAll(filter);
                    // Check if there is object selected
                    if (ssPrompt.Status == PromptStatus.OK)
                    {
                        string layer = "", ltype = "";
                        string coords = "";
                        double len = 0.0;
                        Polyline pline = new Polyline();
                        bool isClosed = false;
                        SelectionSet ss = ssPrompt.Value;

                        //sql = "INSERT INTO dbo.Lines (StartPtX, StartPtY, EndPtX, EndPtY, Layer, Color, Linetype, Length) VALUES (" + startPtX + "," + startPtY + "," + endPtX + "," + endPtY + ",'" + layer + "','" + color + "','" + ltype + "'," + len + ")";
                        string sql = @"INSERT INTO dbo.Plines (Layer, Linetype, Length, Coordinates, IsClosed, Created) 
                                VALUES(@Layer, @Linetype, @Length, @Coordinates, @IsClosed, @Created)";
                        conn.Open();

                        // Loop through the selection set and insert into database one polyline object at a time
                        foreach (SelectedObject sObj in ss)
                        {
                            pline = trans.GetObject(sObj.ObjectId, OpenMode.ForRead) as Polyline;
                            layer = pline.Layer;
                            ltype = pline.Linetype;
                            len = pline.Length;
                            isClosed = pline.Closed;

                            coords = CommonUtil.GetPolylineCoordinates(pline);

                            SqlCommand cmd = new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@Layer", layer);
                            cmd.Parameters.AddWithValue("@Linetype", ltype);
                            cmd.Parameters.AddWithValue("@Length", len);
                            cmd.Parameters.AddWithValue("@Coordinates", coords);
                            cmd.Parameters.AddWithValue("@IsClosed", isClosed);
                            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        ed.WriteMessage("No object selected.");
                    }
                    trans.Commit();

                }

                result = "Done. Completed successfully!";
            }
            catch (System.Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return result;
        }

        #endregion



        // Load all the Blocks (No Attributes) Objects
        #region Load Blocks No Attributes

        public string LoadBlocksNoAttributes()
        {
            string result = "";
            SqlConnection conn = DbUtil.GetConnection();
            try
            {
                // Get the Document and Editor object
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    try
                    {
                        TypedValue[] tv = new TypedValue[1];
                        tv.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
                        SelectionFilter filter = new SelectionFilter(tv);

                        PromptSelectionResult ssPrompt = ed.SelectAll(filter);
                        // Check if there is object selected
                        if (ssPrompt.Status == PromptStatus.OK)
                        {
                            double insPtX = 0.0, insPtY = 0.0;
                            string blkName = "";
                            string layer = "";
                            double rotation = 0.0;
                            string blockId = "";
                            BlockReference blk;
                            string insPt = "";
                            SelectionSet ss = ssPrompt.Value;

                            
                            string sql = @"INSERT INTO dbo.BlocksNoAttributes (insertionPt, BlockName, Layer, Rotation, Created,BlockId) 
                                        VALUES(@InsertionPt, @BlockName, @Layer, @Rotation, @Created , @BlockId)";
                            // Open DB connection
                            conn.Open();

                            // Loop through the selection set and insert into database one block object at a time
                            foreach (SelectedObject sObj in ss)
                            {
                                blk = trans.GetObject(sObj.ObjectId, OpenMode.ForRead) as BlockReference;
                                if (blk.AttributeCollection.Count == 0 & !blk.Name.Contains("*"))
                                {
                                    insPtX = blk.Position.X;
                                    insPtY = blk.Position.Y;
                                    insPt = insPtX.ToString() + "," + insPtY.ToString();
                                    blkName = blk.Name;
                                    layer = blk.Layer;
                                    rotation = blk.Rotation;
                                    blockId = blk.Handle.ToString();

                                    SqlCommand cmd = new SqlCommand(sql, conn);
                                    cmd.Parameters.AddWithValue("@InsertionPt", insPt);
                                    cmd.Parameters.AddWithValue("@BlockName", blkName);
                                    cmd.Parameters.AddWithValue("@Layer", layer);
                                    cmd.Parameters.AddWithValue("@Rotation", rotation);
                                    cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@BlockId", blockId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Abort();
                        throw new Exception($"Transaction failed: {ex.Message}", ex);
                    }
                }
                result = "Done. Completed successfully!";
            }
            catch (System.Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return result;
        }
        #endregion



        // Load all the Blocks (With Attributes) Objects
        #region Load Blocks With Attributes

        public string LoadBlocksWithAttributes()
        {
            string result = "";
            SqlConnection conn = DbUtil.GetConnection();
            try
            {
                // Get the Document and Editor object
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    try
                    {
                        TypedValue[] tv = new TypedValue[1];
                        tv.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
                        SelectionFilter filter = new SelectionFilter(tv);

                        PromptSelectionResult ssPrompt = ed.SelectAll(filter);
                        // Check if there is object selected
                        if (ssPrompt.Status == PromptStatus.OK)
                        {
                            double insPtX = 0.0, insPtY = 0.0;
                            string blkName = "";
                            string layer = "";
                            double rotation = 0.0;
                            string blockId = "";
                            BlockReference blk;
                            string insPt = "";
                            string attributes = "";
                            string sql = "";
                            SelectionSet ss = ssPrompt.Value;

                            //sql = "INSERT INTO dbo.MTexts (insPtX, insPtY, Layer, Color, TextStyle, Height, Width, Text) VALUES (" + insPtX + "," + insPtX + "," + layer + "," + color + ",'" + textstyle + "','" + height + "','" + width + ",'" + tx + "')";
                            sql = @"INSERT INTO dbo.BlocksWithAttributes (insertionPt, BlockName, Layer, Rotation, Attributes, Created ,BlockId) 
                                        VALUES(@InsertionPt, @BlockName, @Layer, @Rotation, @Attributes, @Created, @BlockId)";
                            conn.Open();

                            // Loop through the selection set and insert into database one block object at a time
                            foreach (SelectedObject sObj in ss)
                            {
                                blk = trans.GetObject(sObj.ObjectId, OpenMode.ForRead) as BlockReference;
                                if (blk.AttributeCollection.Count > 0 & !blk.Name.Contains("*"))
                                {
                                    insPtX = blk.Position.X;
                                    insPtY = blk.Position.Y;
                                    insPt = insPtX.ToString() + "," + insPtY.ToString();
                                    blkName = blk.Name;
                                    layer = blk.Layer;
                                    rotation = blk.Rotation;
                                    blockId = blk.Handle.ToString();

                                    // Loop through the Block Attributes
                                    foreach (ObjectId attRefId in blk.AttributeCollection)
                                    {
                                        DBObject obj = trans.GetObject(attRefId, OpenMode.ForRead);
                                        AttributeReference attRef = obj as AttributeReference;
                                        if (attRef != null)
                                        {
                                            //attributes += attRef.TextString + ",";
                                            //attributes += attRef.Tag + "=" + attRef.TextString + "|" + "Rotation=" + attRef.Rotation + ",";
                                            attributes += attRef.Tag + "=" + attRef.TextString + ",";
                                        }
                                    }
                                    attributes = attributes.Substring(0, attributes.Length - 1);

                                    SqlCommand cmd = new SqlCommand(sql, conn);
                                    cmd.Parameters.AddWithValue("@InsertionPt", insPt);
                                    cmd.Parameters.AddWithValue("@BlockName", blkName);
                                    cmd.Parameters.AddWithValue("@Layer", layer);
                                    cmd.Parameters.AddWithValue("@Rotation", rotation);
                                    cmd.Parameters.AddWithValue("@Attributes", attributes);
                                    cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@BlockId", blockId);
                                    cmd.ExecuteNonQuery();

                                    attributes = "";
                                }
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an exception
                        trans.Abort();
                        throw new Exception($"Transaction failed: {ex.Message}", ex);
                    }
                }
                result = "Done. Completed successfully!";
            }
            catch (System.Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return result;
        }

        #endregion

    }
}
