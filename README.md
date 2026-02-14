
## A2AAgentDotNet

### Overview

A2AAgentDotNet is a .NET-based implementation of an agent that can process JSON-RPC requests. It is designed to handle various tasks, such as fetching weather forecasts, by utilizing a modular architecture that allows for easy extension and customization.

### Building and Running the Application

To build and run the application, you can use Docker. The provided `dockerfile` allows you to create a Docker image for the application. Here are the steps to build and run the Docker container:

1. Build the Docker image:
   ```bash
    docker build -t a2a-dotnet-agent:local .
   ```
2. Run the Kubernetes container:
   ```bash
    kubectl apply -f a2a-local.yaml
   ```

### Example Usage

```bash
curl -X POST http://localhost:30080/rpc \
-H "Content-Type: application/json" \
-d '{"jsonrpc":"2.0","method":"get_forecast","params":{"city":"Shinjuku"},"id":1}' && echo ""
# result
# {"jsonrpc":"2.0","result":{"temperature":"22C","forecast":"Sunny","location":"Shinjuku"},"id":1}
```