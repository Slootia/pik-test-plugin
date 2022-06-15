namespace PikTestPlugin.Models
{
    public static class ParametersNames
    {
        private const string _roomNumberOfRoomsParameterName = "ROM_Подзона";
        private const string _apartmentParameterName = "ROM_Зона";
        private const string _apartmentPurposeParameterName = "Квартира";
        private const string _sectionParameterName = "BS_Блок";
        private const string _roomCalculatedSubzoneIdParameterName = "ROM_Расчетная_подзона_ID";
        private const string _roomSubzoneIndexParameterName = "ROM_Подзона_Index";
        private const string _roomSufixToPaint = ".Полутон";

        public static string RoomCalculatedSubzoneIdParameterName => _roomCalculatedSubzoneIdParameterName;
        public static string RoomSubzoneIndexParameterName => _roomSubzoneIndexParameterName;
        public static string RoomSufixToPaint => _roomSufixToPaint;
        public static string RoomNumberOfRoomsParameterName => _roomNumberOfRoomsParameterName;
        public static string ApartmentParameterName => _apartmentParameterName;
        public static string ApartmentPurposeParameterName => _apartmentPurposeParameterName;
        public static string SectionParameterName => _sectionParameterName;
    }
}
