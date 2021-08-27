namespace AplicacionConsola
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        private static void Main(string[] args)
        {
            //Tabla de verdad XOR
            int[,] xorentra = { { 1, 1 }, { 1, 0 }, { 0, 1 }, { 0, 0 } };
            int[] xorsale = { 0, 1, 1, 0 };

            var numEntradas = 2; //Número de entradas
            var capa0 = 4; //Total neuronas en la capa 0
            var capa1 = 4; //Total neuronas en la capa 1
            var capa2 = 1; //Total neuronas en la capa 2
            var perceptron = new Perceptron(numEntradas, capa0, capa1, capa2);

            //Estas serán las entradas externas al perceptrón
            var entradas = new List<double>();
            entradas.Add(0);
            entradas.Add(0);

            //Estas serán las salidas esperadas externas al perceptrón
            var salidaEsperada = new List<double>();
            salidaEsperada.Add(0);

            //Ciclo que entrena la red neuronal
            var totalCiclos = 8000; //Ciclos de entrenamiento
            for (var ciclo = 1; ciclo <= totalCiclos; ciclo++)
            {
                //Por cada ciclo, se entrena el perceptrón con toda la tabla de XOR

                //Cada 200 ciclos muestra como progresa el entrenamiento
                if (ciclo % 200 == 0)
                {
                    Console.WriteLine("Ciclo: " + ciclo);
                }

                for (var conjunto = 0; conjunto < xorsale.Length; conjunto++)
                {
                    //Entradas y salidas esperadas
                    entradas[0] = xorentra[conjunto, 0];
                    entradas[1] = xorentra[conjunto, 1];
                    salidaEsperada[0] = xorsale[conjunto];

                    //Primero calcula la salida del perceptrón con esas entradas
                    perceptron.calculaSalida(entradas);

                    //Luego entrena el perceptrón para ajustar los pesos y umbrales
                    perceptron.Entrena(entradas, salidaEsperada);

                    //Cada 200 ciclos muestra como progresa el entrenamiento
                    if (ciclo % 200 == 0)
                    {
                        perceptron.SalidaPerceptron(entradas, salidaEsperada[0]);
                    }
                }
            }

            Console.WriteLine("Finaliza");
            Console.ReadKey();
        }
    }
}
