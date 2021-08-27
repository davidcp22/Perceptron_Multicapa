namespace AplicacionConsola
{
    using System;
    using System.Collections.Generic;

    public class Neurona
    {
        public List<double> nuevospesos; //Nuevos pesos dados por el algoritmo de "backpropagation"
        public double nuevoumbral; //Nuevo umbral dado por el algoritmo de "backpropagation"
        public List<double> pesos; //Los pesos para cada entrada
        public double umbral; //El peso del umbral

        //Inicializa los pesos y umbral con un valor al azar
        public Neurona(Random azar, int totalEntradas)
        {
            pesos = new List<double>();
            nuevospesos = new List<double>();
            for (var cont = 0; cont < totalEntradas; cont++)
            {
                pesos.Add(azar.NextDouble());
                nuevospesos.Add(0);
            }

            umbral = azar.NextDouble();
            nuevoumbral = 0;
        }

        //Calcula la salida de la neurona dependiendo de las entradas
        public double calculaSalida(List<double> entradas)
        {
            double valor = 0;
            for (var cont = 0; cont < pesos.Count; cont++)
            {
                valor += entradas[cont] * pesos[cont];
            }

            valor += umbral;
            return 1 / (1 + Math.Exp(-valor));
        }

        //Reemplaza viejos pesos por nuevos
        public void actualiza()
        {
            for (var cont = 0; cont < pesos.Count; cont++)
            {
                pesos[cont] = nuevospesos[cont];
            }

            umbral = nuevoumbral;
        }
    }
}
