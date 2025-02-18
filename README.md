# SolarSystemSimulation

## Overview

SolarSystemSimulation is a C# application that visually simulates the motion of celestial bodies in a solar system. The simulation uses **OpenTK** for rendering and physics calculations, displaying planets orbiting the Sun based on real gravitational forces.

## Features

- Realistic gravitational physics for planetary motion.
- 3D rendering of the solar system using **OpenTK**.
- Support for textures to enhance visualization.
- Adjustable simulation speed and zoom levels.
- Toggleable background and data display for enhanced visualization.
- User input handling for interaction.

## Installation

### Prerequisites

Ensure you have the following installed:

- .NET 6.0 or later
- OpenTK library (`OpenTK.Graphics`)

### Cloning the Repository

```sh
git clone https://github.com/n1k0m0/SolarSystemSimulation
cd SolarSystemSimulation
```

### Building the Project

1. Open the project in **Visual Studio** or **JetBrains Rider**.
2. Restore dependencies using:
   ```sh
   dotnet restore
   ```
3. Build the solution:
   ```sh
   dotnet build
   ```

### Running the Simulation

To start the simulation, run:

```sh
dotnet run
```

Alternatively, you can execute the compiled binary from the **bin** directory.

## Usage

### Controls

- **Arrow Keys:** Adjust zoom and scale.
- **Page Up/Down:** Change simulation speed.
- **F1:** Reset the simulation.
- **F2:** Toggle planetary data display.
- **F3:** Toggle background.
- **Escape:** Exit the simulation.

## Project Structure

```
SolarSystemSimulation/
¦-- Program.cs               # Entry point for the simulation
¦-- SolarSystemWindow.cs     # Handles rendering and user interaction
¦-- AstronomicalBody.cs      # Defines celestial bodies and physics calculations
¦-- Textures/                # Folder containing planet textures
¦-- LICENSE                  # Licensing information
¦-- README.md                # This file
```

## License

This project is licensed under the Apache License 2.0. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- **OpenTK** for 3D rendering and graphics support.
- NASA/JPL for accurate planetary data.

## Contributing

Pull requests and suggestions are welcome! If you find a bug or have ideas for enhancements, feel free to open an issue.
