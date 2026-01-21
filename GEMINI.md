# Sample ADK .NET MCP Project

## Project Overview

This project demonstrates the integration of the **Google Agent Development Kit (ADK)** with the **Model Context Protocol (MCP)**. It features a Python-based ADK agent that consumes tools provided by a local MCP server implemented in .NET (C#).

The system consists of two main components:
1.  **Weather MCP Server (`weather/`)**: A .NET console application acting as an MCP server over Stdio. It exposes tools to fetch location and weather data using the Open-Meteo API.
2.  **ADK Agent (`adk-sample/`)**: A Python project defining an `LlmAgent` configured to communicate with the .NET MCP server.

## Directory Structure

*   `weather/`: Contains the .NET MCP server source code.
    *   `Program.cs`: Entry point, configures the MCP server and tools.
    *   `WeatherTools.cs`: Implementation of `GetLocation` and `GetForecast` tools.
    *   `weather.csproj`: .NET project file (targets .NET 10.0).
*   `adk-sample/`: Contains the Python ADK agent definition.
    *   `mcp_agent/agent.py`: Defines the `root_agent` and configures the `McpToolset` to spawn the .NET server.
    *   `inspect_agent.py`: A utility script to inspect the agent object.
    *   `pyproject.toml`: Python dependencies (requires `google-adk`).

## Prerequisites

*   **.NET SDK**: The project targets .NET 10.0 (Preview). Ensure you have a compatible SDK installed.
*   **Python**: Version 3.12 or higher.
*   **uv**: Recommended for Python dependency management.

## Setup and Usage

### 1. Build the .NET Server

Before running the agent, ensure the .NET project builds successfully.

```bash
dotnet build weather/weather.csproj
```

### 2. Configure Python Environment

Navigate to the `adk-sample` directory and install dependencies.

```bash
cd adk-sample
uv sync
```

### 3. Agent Configuration

The agent is defined in `adk-sample/mcp_agent/agent.py`.
**Note:** The path to the .NET project in `agent.py` is currently hardcoded to:
`/home/uttam/sample-adk-dotnet-mcp/weather`

If you move the project, you must update the `args` in the `StdioServerParameters` within `agent.py` to point to the correct location of the `weather` directory.

### 4. Running the Agent

You can verify the agent configuration by running the inspection script:

```bash
cd weather
dotnet run
```

```bash
cd adk-sample
adk web
```


(Note: Actual interaction with the agent typically requires an ADK runner or a custom script to invoke the agent's `generate_response` or similar methods, which is not explicitly provided in the root of this sample.)

## Key Technologies

*   **Google ADK (`google-adk`)**: Framework for building AI agents.
*   **Model Context Protocol (MCP)**: Standard for connecting AI models to external tools.
*   **Open-Meteo API**: Public weather data source used by the .NET tools.
