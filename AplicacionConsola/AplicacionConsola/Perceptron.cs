namespace AplicacionConsola
{
    using System;
    using System.Collections.Generic;

    public class Perceptron
    {
        public List<Capa> capas;

        //Crea las diversas capas
        public Perceptron(int numEntrada, int capa0, int capa1, int capa2)
        {
            Random azar = new Random();
            capas = new List<Capa>();
            capas.Add(new Capa(azar, capa0, numEntrada)); //Crea la capa 0
            capas.Add(new Capa(azar, capa1, capa0)); //Crea la capa 1
            capas.Add(new Capa(azar, capa2, capa1)); //Crea la capa 2
        }

        //Imprime los datos de las diferentes capas
        public void SalidaPerceptron(List<double> entradas, double salidaesperada)
        {
            for (var cont = 0; cont < entradas.Count; cont++)
            {
                Console.Write(entradas[cont].ToString() + " | ");
            }

            Console.Write(" Esperada: " + salidaesperada + "  Calculada: ");
            for (var cont = 0; cont < capas[2].salidas.Count; cont++)
            {
                if (capas[2].salidas[cont] >= 0.5)
                {
                    Console.Write(" 1 | ");
                }
                else
                {
                    Console.Write(" 0 | ");
                }

                Console.Write(capas[2].salidas[cont].ToString() + " | ");
            }

            Console.WriteLine(" ");
        }

        //Dada las entradas al perceptrón, se calcula la salida de cada capa.
        //Con eso se sabrá que salidas se obtienen con los pesos y umbrales actuales.
        //Esas salidas son requeridas para el algoritmo de entrenamiento.
        public void calculaSalida(List<double> entradas)
        {
            capas[0].CalculaCapa(entradas);
            capas[1].CalculaCapa(capas[0].salidas);
            capas[2].CalculaCapa(capas[1].salidas);
        }

        //Con las salidas previamente calculadas con unas determinadas entradas
        //se ejecuta el algoritmo de entrenamiento "Backpropagation"  
        public void Entrena(List<double> entradas, List<double> salidaEsperada)
        {
            int capa0 = capas[0].neuronas.Count;
            int capa1 = capas[1].neuronas.Count;
            int capa2 = capas[2].neuronas.Count;

            //Factor de aprendizaje
            var alpha = 0.2;

            //Procesa pesos capa 2
            for (var j = 0; j < capa1; j++) //Va de neurona en neurona de la capa 1
            for (var i = 0; i < capa2; i++)
            {
                //Va de neurona en neurona de la capa de salida (capa 2)
                double Yi = capas[2].salidas[i]; //Salida de la neurona de la capa de salida
                double Si = salidaEsperada[i]; //Salida esperada
                double a1j = capas[1].salidas[j]; //Salida de la capa 1
                var dE2 = a1j * (Yi - Si) * Yi * (1 - Yi); //La fórmula del error
                capas[2].neuronas[i].nuevospesos[j] =
                    capas[2].neuronas[i].pesos[j] - alpha * dE2; //Ajusta el nuevo peso
            }

            //Procesa pesos capa 1
            for (var j = 0; j < capa0; j++) //Va de neurona en neurona de la capa 0
            for (var k = 0; k < capa1; k++)
            {
                //Va de neurona en neurona de la capa 1
                double acum = 0;
                for (var i = 0; i < capa2; i++)
                {
                    //Va de neurona en neurona de la capa 2
                    double Yi = capas[2].salidas[i]; //Salida de la capa 2
                    double Si = salidaEsperada[i]; //Salida esperada
                    double W2ki = capas[2].neuronas[i].pesos[k];
                    acum += W2ki * (Yi - Si) * Yi * (1 - Yi); //Sumatoria
                }

                double a0j = capas[0].salidas[j];
                double a1k = capas[1].salidas[k];
                var dE1 = a0j * a1k * (1 - a1k) * acum;
                capas[1].neuronas[k].nuevospesos[j] = capas[1].neuronas[k].pesos[j] - alpha * dE1;
            }

            //Procesa pesos capa 0
            for (var j = 0; j < entradas.Count; j++) //Va de entrada en entrada
            for (var k = 0; k < capa0; k++)
            {
                //Va de neurona en neurona de la capa 0
                double acumular = 0;
                for (var p = 0; p < capa1; p++)
                {
                    //Va de neurona en neurona de la capa 1
                    double acum = 0;
                    for (var i = 0; i < capa2; i++)
                    {
                        //Va de neurona en neurona de la capa 2
                        double Yi = capas[2].salidas[i];
                        double Si = salidaEsperada[i]; //Salida esperada
                        double W2pi = capas[2].neuronas[i].pesos[p];
                        acum += W2pi * (Yi - Si) * Yi * (1 - Yi); //Sumatoria interna
                    }

                    double W1kp = capas[1].neuronas[p].pesos[k];
                    double a1p = capas[1].salidas[p];
                    acumular += W1kp * a1p * (1 - a1p) * acum; //Sumatoria externa
                }

                double xj = entradas[j];
                double a0k = capas[0].salidas[k];
                var dE0 = xj * a0k * (1 - a0k) * acumular;
                double W0jk = capas[0].neuronas[k].pesos[j];
                capas[0].neuronas[k].nuevospesos[j] = W0jk - alpha * dE0;
            }


            //Procesa umbrales capa 2
            for (var i = 0; i < capa2; i++)
            {
                //Va de neurona en neurona de la capa de salida (capa 2)
                double Yi = capas[2].salidas[i]; //Salida de la neurona de la capa de salida
                double Si = salidaEsperada[i]; //Salida esperada
                var dE2 = (Yi - Si) * Yi * (1 - Yi);
                capas[2].neuronas[i].nuevoumbral = capas[2].neuronas[i].umbral - alpha * dE2;
            }

            //Procesa umbrales capa 1
            for (var k = 0; k < capa1; k++)
            {
                //Va de neurona en neurona de la capa 1
                double acum = 0;
                for (var i = 0; i < capa2; i++)
                {
                    //Va de neurona en neurona de la capa 2
                    double Yi = capas[2].salidas[i]; //Salida de la capa 2
                    double Si = salidaEsperada[i];
                    double W2ki = capas[2].neuronas[i].pesos[k];
                    acum += W2ki * (Yi - Si) * Yi * (1 - Yi);
                }

                double a1k = capas[1].salidas[k];
                var dE1 = a1k * (1 - a1k) * acum;
                capas[1].neuronas[k].nuevoumbral = capas[1].neuronas[k].umbral - alpha * dE1;
            }

            //Procesa umbrales capa 0
            for (var k = 0; k < capa0; k++)
            {
                //Va de neurona en neurona de la capa 0
                double acumular = 0;
                for (var p = 0; p < capa1; p++)
                {
                    //Va de neurona en neurona de la capa 1
                    double acum = 0;
                    for (var i = 0; i < capa2; i++)
                    {
                        //Va de neurona en neurona de la capa 2
                        double Yi = capas[2].salidas[i];
                        double Si = salidaEsperada[i];
                        double W2pi = capas[2].neuronas[i].pesos[p];
                        acum += W2pi * (Yi - Si) * Yi * (1 - Yi);
                    }

                    double W1kp = capas[1].neuronas[p].pesos[k];
                    double a1p = capas[1].salidas[p];
                    acumular += W1kp * a1p * (1 - a1p) * acum;
                }

                double a0k = capas[0].salidas[k];
                var dE0 = a0k * (1 - a0k) * acumular;
                capas[0].neuronas[k].nuevoumbral = capas[0].neuronas[k].umbral - alpha * dE0;
            }

            //Actualiza los pesos
            capas[0].actualiza();
            capas[1].actualiza();
            capas[2].actualiza();
        }
    }
}
