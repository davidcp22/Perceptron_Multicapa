namespace AplicacionConsola
{
    using System;
    using System.Collections.Generic;

    public class Capa
    {
        public List<Neurona> neuronas; //Las neuronas que tendrá la capa
        public List<double> salidas; //Almacena las salidas de cada neurona

        public Capa(Random azar, int totalNeuronas, int totalEntradas)
        {
            neuronas = new List<Neurona>();
            salidas = new List<double>();
            //Genera las neuronas
            for (var cont = 0; cont < totalNeuronas; cont++)
            {
                neuronas.Add(new Neurona(azar, totalEntradas));
                salidas.Add(0);
            }
        }

        //Calcula las salidas de cada neurona de la capa
        public void CalculaCapa(List<double> entradas)
        {
            for (var cont = 0; cont < neuronas.Count; cont++)
            {
                salidas[cont] = neuronas[cont].calculaSalida(entradas);
            }
        }

        //Actualiza los pesos y umbrales de las neuronas
        public void actualiza()
        {
            for (var cont = 0; cont < neuronas.Count; cont++)
            {
                neuronas[cont].actualiza();
            }
        }
    }
}
