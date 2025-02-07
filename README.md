# SalesReportAnalysis
 
# Sales Report Analysis

## Prerequisites
- These pre-requisite would help Running and Testing the Web App when you don't have Visual Studio
- Install [VS Code](https://code.visualstudio.com/).
- Clone the GitHub repository to your local machine.
- Install the following extensions in VS Code:
  1. [ .NET Install Tool](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.vscode-dotnet-runtime)
  2. [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
  3. [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
  4. [Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)


## Steps to Execute the Web API

1. **Navigate to the Repository Path**
   - Git repo base path: `[local git path]\Implinj\ImplinjSalesAnalysis`

2. **Open Terminal in VS Code**
   - Open VS Code.
   - Use the menu bar at the top to navigate to **View** > **Terminal**.
   - The **Terminal** tab will appear at the bottom.

3. **Change Directory**
   - Use the following command to change the directory to `Implinj\ImplinjSalesAnalysis`:
     ```sh
     cd [local git path]\Implinj\ImplinjSalesAnalysis
     ```

4. **Build and Run the Web API**
   - Run the following commands in the terminal:
     ```sh
     dotnet build
     dotnet run
     ```

5. **Verify Successful Execution**
   - After the .NET service runs successfully, it will show a success log in the terminal.

6. **Send HTTP Requests**
   - Open the file `[local git path]\Implinj\ImplinjSalesAnalysis\ImplinjSalesAnalysis.http` in VS Code.
   - You can now send requests by clicking the appropriate buttons.
   - Refer to the image file `[local git path]\Implinj\ImplinjSalesAnalysis\Documentation\HttpRequest-Execute-Guide.jpg` to see where to click to initiate Scan and Query requests.
   - The results will be displayed in the right pane of VS Code.





The Web API supports the following operations:

### 1. GET - GetSalesRecordSummary
- **Description**: Takes the input as a CSV file name. The file needs to be added to the "Data" folder in the project "ImplinjSalesAnalysis". If the file is not present, it will return the error "Could not find file".
- **URL**: `http://localhost:5064/api/SalesRecordAnalysis/GetSummary?fileName=SalesRecords.csv`

### 2. POST - UploadFileAndGetSalesRecordSummary
- **Description**: This POST API accepts the CSV file as an attachment. I have tested this using `ImpinjSalesAnalysis.http`. After running the project, you need to click on "Send Request".
- **URL**: `http://localhost:5064/api/SalesRecordAnalysis/PostSummary`
- **Note**: You need to send the attachment, tested using the `.http` file.

## Build and Run the Project

- **IDE**: Use Visual Studio Community Edition.
- **Notes**: 
  - Use HTTP to avoid any certificate issues on local while running the project.
  - After running the project, you can use `ImpinjSalesAnalysis.http` to trigger the HTTP calls.
  - The functionality has been validated using Unit tests.

## Usage

1. **Clone the Repository**:
   ```sh
   git clone <repository-url>




