# Light Board CLI

This project was developed to control a light board via a serial connection. It offers functionality such as retrieving device information, sending text, and exploring memory. Implementations in both C# and Java are available.

---

## How this project was created

### Step 1: Basic configuration
To control the light board, the serial connection was configured first. After experimenting with various settings, the correct configuration was determined to be:
- **Baudrate**: 4800
- **Parity**: Even
- **Data bits**: 8
- **Stop bits**: 1

### Step 2: Research and implementation
Small programs were initially written to test the device's responses. Simple read commands were sent to verify communication, and the received data was analyzed and interpreted step by step.

### Step 3: Sending text
The light board accepts text in IBM437 encoding. A function was added to send text, with optional extra spaces to adjust how the text is displayed on the device.

---

## Memory exploration

### Functionality
The project includes several options to explore the memory of the light board:

1. **Exploring memory pages**  
   Retrieves the content of specific memory pages. This helps gain insight into the structure of the memory.

2. **Optimized memory exploration**  
   This function searches memory sequentially by addresses. Each address is checked for relevant data, helping identify useful memory locations.

3. **Viewing the memory map**  
   Displays an overview of the entire memory, showing the contents of each memory address.

4. **Searching memory**  
   Allows searching for a specific term within the memory. Results indicate where in the memory the entered text is found.

---

## Project status

Although much functionality is already available, there are still unanswered questions about the memory and internal structure of the light board. To gain further clarity, contact has been made with the device's manufacturer. Their feedback could enable further improvements and extensions.

---

## How to use

1. Connect the light board to an available COM port.
2. Adjust the COM port setting in the code to match the correct port.
3. Compile and run the program.
4. Select an option in the CLI to interact with the light board.

---

## Future improvements
- Gaining more insight into the memory after feedback from the manufacturer.
- Enhanced memory search functionality.
- Potential GUI support to make the light board more user-friendly.
- Improved error handling and logging.
