# MKS ServoD Control V2

A modern, cross-platform .NET 10 Avalonia UI application for controlling and monitoring MKS ServoD stepper motors. 
This project is a complete refactor and upgrade from the legacy Windows Forms `MKS_ServoD_Control` application, bringing professional MVVM architecture, robust serial communication, and a beautiful, responsive user interface.

## 🚀 Features

*   **Full Feature Parity**: Supports all commands from the legacy application (Motor Control, System Parameters, Homing & Limits, Status Monitoring).
*   **Modern MVVM Architecture**: Built with `CommunityToolkit.Mvvm` and Dependency Injection for maintainable and testable code.
*   **Cross-Platform Serial Port Support**: Uses `RJCP.IO.Ports.SerialPortStream` for stable serial communication across Windows, macOS, and Linux.
*   **Premium UI/UX**: Designed with Avalonia UI, featuring a clean, responsive, and data-bound interface divided into logical workspaces.
*   **Robust Protocol Layer**: Strongly typed Enums and dedicated `MksCommandBuilder` / `MksResponseParser` ensure reliable data transmission and checksum validation.

## 🏗️ Project Structure

The codebase is organized by domain responsibilities:

*   **`Core/`**: Protocol parsing (`MksResponseParser`), command building (`MksCommandBuilder`), opcodes (`MksProtocolConstants`), and strongly-typed definitions (`MksEnums`).
*   **`Services/`**: Interfaces and implementations for hardware communication (`SerialService`, `MotorControlService`).
*   **`Models/`**: State representation and configuration POCOs (`ConnectionConfig`, `MotorState`).
*   **`ViewModels/`**: Presentation logic handling state and user commands asynchronously using `[RelayCommand]` and `[ObservableProperty]`.
*   **`Views/`**: Avalonia XML (`.axaml`) files defining the visual interface, utilizing UserControls for a modular layout.

## 🛠️ Requirements & Setup

1.  **Framework**: [.NET 10 SDK](https://dotnet.microsoft.com/)
2.  **IDE**: Visual Studio 2022, JetBrains Rider, or VS Code with C# Dev Kit and Avalonia extensions.
3.  **Dependencies**: 
    *   `Avalonia`
    *   `CommunityToolkit.Mvvm`
    *   `Microsoft.Extensions.DependencyInjection`
    *   `SerialPortStream`

### Build & Run

Ensure your motor or serial adapter is connected.

```bash
# Compile the project
dotnet build

# Run the UI
dotnet run
```

## 🖥️ User Interface Workspaces

The application is structured into the following key areas:

1.  **Connection Pane**: Manage COM Port, Baud Rate, and Device Address.
2.  **Monitor Pane**: Real-time readouts of Encoder values, Pulses, Error Flags, and hardware state.
3.  **Basic Control (Tab)**: Interactive Velocity/Speed controls and Absolute/Relative position commands.
4.  **Parameters (Tab)**: Configure system properties like Subdivision, Run/Hold Currents, Accelerations, and RS485 settings.
5.  **Homing & Limits (Tab)**: Single-turn calibration and limit switch trigger setups.

## 📜 Migration Notes
This `V2` project explicitly replaces `Form1.cs`. If upgrading from the older system, note that `System.IO.Ports` was replaced to ensure Linux/macOS users do not experience random disconnects or buffer lockups.
