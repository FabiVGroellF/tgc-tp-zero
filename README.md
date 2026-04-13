# TGC - MonoGame - TP Zero

[![.NET](https://github.com/tgc-utn/tgc-monogame-tp-zero/actions/workflows/dotnet.yml/badge.svg)](https://github.com/tgc-utn/tgc-monogame-tp-zero/actions/workflows/dotnet.yml)
[![GitHub license](https://img.shields.io/github/license/tgc-utn/tgc-monogame-tp-zero.svg)](https://github.com/tgc-utn/tgc-monogame-tp-zero/blob/master/LICENSE)

[#BuiltWithMonoGame](https://monogame.net/) and [.NET Core](https://dotnet.microsoft.com)

# Descripción

Trabajo práctico cero de la asignatura electiva [Técnicas de Gráficos por Computadora](https://tgc-utn.github.io/) (TGC) en la carrera de Ingeniería en Sistemas de Información. Universidad Tecnológica Nacional, Facultad Regional Buenos Aires (UTN-FRBA).

## Consigna

- Cargar el modelo del auto y renderizarlo
- Implementar movimientos para el auto
  - <kbd>w</kbd> y <kbd>s</kbd> para acelerar y desacelerar
  - <kbd>a</kbd> y <kbd>d</kbd> para girar el auto

Bonus:
- Movimiento con aceleración y velocidad por separado
- <kbd>espacio</kbd> para saltar

__No es necesario modificar la c&aacute;mara, la misma apunta al costado de la matriz CarWorld de manera intencional.__

## Configuración del entorno de desarrollo

Pueden encontrar los pasos a seguir según su sistema operativo en el siguiente documento [install.md](https://github.com/tgc-utn/tgc-monogame-samples/blob/master/docs/install/install.md).

Afuera del mundo Windows, vas a necesitar la ayudar de [Wine](https://www.winehq.org) para los shaders, por lo menos por [ahora](https://github.com/MonoGame/MonoGame/issues/2167).

Los recursos usados se almacenan utilizando [Git LFS](https://git-lfs.github.com), con lo cual antes de clonar el repositorio les conviene tenerlo instalado así es automático el pull o si ya lo tienen pueden hacer `git lfs pull`.
