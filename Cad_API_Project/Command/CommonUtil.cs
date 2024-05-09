using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;

namespace Cad_API_Project.Command
{
    public static class CommonUtil
    {
        // Get all the vertices of the Polyline and return it to the calling method in comma separated value
        #region PolyLine Coordinates

        public static string GetPolylineCoordinates(Polyline _pLines)
        {
            var _verticesCount = _pLines.NumberOfVertices;
            Point2d _coordinate;
            string _coordinates = "";
            int i;
            for (i = 0; i <= _verticesCount - 1; i++)
            {
                _coordinate = _pLines.GetPoint2dAt(i);
                _coordinates += _coordinate[0].ToString() + "," + _coordinate[1].ToString();
                if ((i < _verticesCount - 1))
                    _coordinates += ",";
            }
            return _coordinates;
        }

        #endregion


        #region Get Color Index

        public static int GetColorIndex(string _colorName)
        {
            int _color = 7;
            switch (_colorName.ToUpper())
            {
                case "RED":
                    _color = 1;
                    break;
                case "YELLOW":
                    _color = 2;
                    break;
                case "GREEN":
                    _color = 3;
                    break;
                case "CYAN":
                    _color = 4;
                    break;
                case "BLUE":
                    _color = 5;
                    break;
                case "MAGENTA":
                    _color = 6;
                    break;
                case "WHITE":
                    _color = 7;
                    break;
                case "BYBLOCK":
                    _color = 0;
                    break;
                case "BYLAYER":
                    _color = 256;
                    break;
                default:
                    _color = 256;
                    break;
            }
            return _color;
        }

        #endregion


        #region Add XData To Entity

        public static void AddXDataToEntity(string _appName, Entity _entity, int _xdValue)
        {
            Document _document = Application.DocumentManager.MdiActiveDocument;
            Database _database = _document.Database;
            Transaction _transation = _database.TransactionManager.StartTransaction();
            using (_transation)
            {
                // Get the registered application names table
                RegAppTable _regAppTable = (RegAppTable)_transation.GetObject(_database.RegAppTableId, OpenMode.ForRead);

                if (!_regAppTable.Has(_appName))
                {
                    _regAppTable.UpgradeOpen();

                    // Add the application name for Xdata
                    RegAppTableRecord _regAppTableRecord = new RegAppTableRecord();
                    _regAppTableRecord.Name = _appName;
                    _regAppTable.Add(_regAppTableRecord);
                    _transation.AddNewlyCreatedDBObject(_regAppTableRecord, true);
                }

                // Append the Xdata to entity
                ResultBuffer _resultBuffer = new ResultBuffer(new TypedValue(1001, _appName), new TypedValue((int)DxfCode.ExtendedDataInteger32, _xdValue));
                _entity.XData = _resultBuffer;
                _resultBuffer.Dispose();
                _transation.Commit();
            }
        }
        #endregion


        #region Read XData From Entity

        public static Int32 ReadXDataFromEntity(string _appName, Entity ent)
        {
            Int32 _id = 0;
            Document _document = Application.DocumentManager.MdiActiveDocument;
            Database _database = _document.Database;
            Transaction _transaction = _database.TransactionManager.StartTransaction();
            using (_transaction)
            {
                ResultBuffer _resultBuffer = ent.GetXDataForApplication(_appName);
                if (_resultBuffer != null)
                {
                    TypedValue[] _typedValue = _resultBuffer.AsArray();
                    foreach (TypedValue _tv in _typedValue)
                    {
                        switch ((DxfCode)_tv.TypeCode)
                        {
                            case DxfCode.ExtendedDataInteger32:
                                _id = Convert.ToInt32(_tv.Value);
                                break;
                        }
                    }
                    _resultBuffer.Dispose();
                }

            }
            return _id;
        }

        #endregion
    }
}
