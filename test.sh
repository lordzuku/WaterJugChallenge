#!/bin/bash

# Base URL for the API
BASE_URL="https://localhost:5001/api/waterjug"

# Function to make a POST request and print the response
test_endpoint() {
    echo "Testing: $1"
    echo "Request: $2"
    echo "Response:"
    curl -X POST "$BASE_URL/solve" \
         -H "Content-Type: application/json" \
         -d "$2" \
         --insecure
    echo -e "\n----------------------------------------\n"
}

# Test cases

# Test 1: Valid request
test_endpoint "Valid request" '{"x_capacity": 2, "y_capacity": 10, "z_amount_wanted": 4}'

# Test 2: Invalid request - negative capacity
test_endpoint "Invalid request - negative capacity" '{"x_capacity": -2, "y_capacity": 10, "z_amount_wanted": 4}'

# Test 3: Invalid request - zero capacity
test_endpoint "Invalid request - zero capacity" '{"x_capacity": 0, "y_capacity": 10, "z_amount_wanted": 4}'

# Test 4: Invalid request - z_amount_wanted greater than both capacities
test_endpoint "Invalid request - z_amount_wanted too large" '{"x_capacity": 2, "y_capacity": 3, "z_amount_wanted": 5}'

# Test 5: Invalid request - invalid JSON format
test_endpoint "Invalid request - invalid JSON" '{"x_capacity": 2, "y_capacity": 10, "z_amount_wanted": 4'

# Test 6: Valid request - no solution possible
test_endpoint "Valid request - no solution" '{"x_capacity": 2, "y_capacity": 4, "z_amount_wanted": 3}' 