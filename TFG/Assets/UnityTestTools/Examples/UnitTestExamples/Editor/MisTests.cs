using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
	[Category("Mis Tests")]
	internal class MisTests
	{
		[Test]
		public void TestEmpaquetado()
		{
			int resultadoEsperado = 141296317; // Equivale a 1000011011000000001010111101
			int resultadoReal = BasicMovementServer.empaquetar2Floats(21.568f, 7.015f);
			Assert.That(resultadoEsperado == resultadoReal);
		}

		[Test]
		public void TestDesempaquetado()
		{
			Vector2 coordenadaEsperada = new Vector2(21.57f, 7.01f);
			int parametroEntrada = 141296317;
			Vector2 resultadoReal = BasicMovementClient.desempaquetar2Floats(parametroEntrada);
			Assert.That(resultadoReal == coordenadaEsperada);
		}

		[Test]
		public void TestEmpaquetarYDesempaquetar()
		{
			Vector2 coordenadaDeEntrada = new Vector2(256.98f, 123.4f);
			int datosEmpaquetados = BasicMovementServer.empaquetar2Floats(coordenadaDeEntrada.x, 
			                                                              coordenadaDeEntrada.y);
			Vector2 coordenadaDeSalida = BasicMovementClient.desempaquetar2Floats(datosEmpaquetados);
			Assert.That(coordenadaDeEntrada == coordenadaDeSalida);
		}
	}
}
