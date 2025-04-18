# Base URL for the API
$baseUrl = "https://localhost:7159/api/waterjug"

# Bypass SSL certificate validation
add-type @"
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    public class TrustAllCertsPolicy : ICertificatePolicy {
        public bool CheckValidationResult(
            ServicePoint srvPoint, X509Certificate certificate,
            WebRequest request, int certificateProblem) {
            return true;
        }
    }
"@
[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

# Function to make a POST request and print the response
function Test-Endpoint {
    param(
        [string]$testName,
        [string]$requestBody
    )
    Write-Host "Testing: $testName"
    Write-Host "Request: $requestBody"
    Write-Host "Response:"
    
    try {
        $response = Invoke-RestMethod -Uri "$baseUrl/solve" `
            -Method Post `
            -ContentType "application/json" `
            -Body $requestBody
        $response | ConvertTo-Json
    }
    catch {
        Write-Host $_.Exception.Message
        if ($_.Exception.Response) {
            Write-Host "Status Code:" $_.Exception.Response.StatusCode.value__
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            Write-Host $reader.ReadToEnd()
        }
    }
    Write-Host "`n----------------------------------------`n"
}

# Test cases

# Test 1: Valid request
Test-Endpoint "Valid request" '{"x_capacity": 2, "y_capacity": 10, "z_amount_wanted": 4}'

# Test 2: Invalid request - negative capacity
Test-Endpoint "Invalid request - negative capacity" '{"x_capacity": -2, "y_capacity": 10, "z_amount_wanted": 4}'

# Test 3: Invalid request - zero capacity
Test-Endpoint "Invalid request - zero capacity" '{"x_capacity": 0, "y_capacity": 10, "z_amount_wanted": 4}'

# Test 4: Invalid request - z_amount_wanted greater than both capacities
Test-Endpoint "Invalid request - z_amount_wanted too large" '{"x_capacity": 2, "y_capacity": 3, "z_amount_wanted": 5}'

# Test 5: Invalid request - invalid JSON format
Test-Endpoint "Invalid request - invalid JSON" '{"x_capacity": 2, "y_capacity": 10, "z_amount_wanted": 4'

# Test 6: Valid request - no solution possible
Test-Endpoint "Valid request - no solution" '{"x_capacity": 2, "y_capacity": 4, "z_amount_wanted": 3}' 