namespace HS.Message.Share.AutoFill
{
    public class MAutoFillField
    {
        public string DataType { get; set; }

        public string KeyFieldName { get; set; }

        public string ValueFieldName { get; set; }

        public MAutoFillField()
        {
        }

        public MAutoFillField(string dataType, string keyFieldName, string valueFieldName)
        {
            DataType = dataType;
            KeyFieldName = keyFieldName;
            ValueFieldName = valueFieldName;
        }
    }
}
