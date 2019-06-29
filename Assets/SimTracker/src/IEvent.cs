namespace SimTracker
{
    //Clase abstracta con atributos generales
    interface IEvent 
    {
        string ToCSV();

        string ToJson();
    }
}
