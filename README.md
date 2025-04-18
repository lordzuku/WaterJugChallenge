# Water Jug Challenge API

This is a .NET Web API that solves the classic Water Jug Challenge. The API accepts the capacities of two jugs and a target amount, then returns the sequence of steps needed to measure exactly the target amount.

## Table of Contents
- [Requirements](#requirements)
- [Setup and Running](#setup-and-running)
- [API Documentation](#api-documentation)
- [Algorithm Design](#algorithm-design)
- [Error Handling](#error-handling)
- [Testing](#testing)
- [Design Decisions](#design-decisions)

## Requirements

- .NET 8.0 or later
- Visual Studio with C# extensions (recommended)
- Git (for version control)

## Setup and Running

1. Clone the repository:
   ```bash
   git clone https://github.com/lordzuku/WaterJugChallenge.git
   cd WaterJugChallenge
   ```

2. Open the solution in Visual Studio or VS Code

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5117`

## API Documentation

### POST /api/WaterJug/solve

Solves the water jug problem for given jug capacities and target amount.

#### Request Body

```json
{
    "x_capacity": 3,
    "y_capacity": 5,
    "z_amount_wanted": 4
}
```

#### Response

```json
{
    "solution": [
        {
            "stepNumber": 1,
            "bucketX": 2,
            "bucketY": 0,
            "action": "Fill bucket X"
        },
        {
            "stepNumber": 2,
            "bucketX": 0,
            "bucketY": 2,
            "action": "Transfer from bucket X to Y"
        },
        {
            "stepNumber": 3,
            "bucketX": 2,
            "bucketY": 2,
            "action": "Fill bucket X"
        },
        {
            "stepNumber": 4,
            "bucketX": 0,
            "bucketY": 4,
            "action": "Transfer from bucket X to Y",
            "status": "Solved"
        }
    ]
}
```

#### Status Codes

- 200 OK: Solution found successfully
- 400 Bad Request: Invalid input parameters
- 500 Internal Server Error: Unexpected server error

## Algorithm Design

The solution implements a dual-path approach to solve the Water Jug Challenge efficiently. Here's a detailed breakdown of the algorithm:

### Input Validation

1. Checks for positive integers in all inputs
2. Verifies target amount doesn't exceed larger jug capacity
3. Uses GCD to determine if solution is possible (if z is not divisible by GCD(x,y), no solution exists)

### Dual-Path Strategy

The algorithm explores two paths simultaneously to find the optimal solution:

1. **Path X**: Starting with bucket X
   - Tracks states using `_visitedStatesX` HashSet to prevent cycles
   - Follows sequence: Fill X → Transfer to Y → Empty Y
   - Returns first valid solution found

2. **Path Y**: Starting with bucket Y
   - Tracks states using `_visitedStatesY` HashSet to prevent cycles
   - Follows sequence: Fill Y → Transfer to X → Empty X
   - Returns first valid solution found

### Operations

The algorithm implements three fundamental operations:
- Fill a jug to its capacity
- Empty a jug completely
- Transfer water between jugs

### Optimization

- Uses HashSet for O(1) state lookups
- Implements cycle detection to prevent infinite loops
- Maximum step limit (1000) to prevent excessive computation
- Returns the first valid solution found, which is guaranteed to be optimal

## Error Handling

The API implements comprehensive error handling for various scenarios:

1. **Input Validation Errors**
   - Non-positive integers
   - Target amount exceeding jug capacity
   - No solution possible (GCD check)

2. **Runtime Errors**
   - Cycle detection in state transitions
   - Maximum step limit exceeded
   - Unexpected server errors

All errors return appropriate HTTP status codes and descriptive error messages.

## Testing

The solution includes comprehensive unit tests covering:

- Input validation
- Algorithm correctness
- Edge cases
- Error scenarios

Run tests using:

```bash

dotnet test
```

## Design Decisions

### Why Dual-Path Approach?

The dual-path strategy was chosen because:

1. It explores both possible starting points (filling X or Y first)
2. Guarantees finding the optimal solution faster
3. Prevents getting stuck in suboptimal paths
4. Provides redundancy in case one path fails

### Why HashSet for State Tracking?

- O(1) lookup time for state existence checks
- Prevents duplicate state processing
- Efficient memory usage
- Enables quick cycle detection

### Why Maximum Step Limit?

- Prevents infinite loops in edge cases
- Ensures API response time remains reasonable
- Protects against potential DoS attacks
- Provides clear failure condition

### Why Separate State Tracking for Each Path?

- Prevents cross-contamination between paths
- Allows independent exploration of both strategies
- Makes the algorithm more maintainable and testable
- Enables potential parallel processing in future versions
