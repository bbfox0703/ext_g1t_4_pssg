# ext_g1t_4_pssg
Extract g1t from .PSSG file (Atelier ayesha DX)
## Introduction
This tool helps you extract G1T files from PSSG files. If you have `.PSSG` files in your folder, this tool will find them and extract the G1T files they contain. Note: This tool cannot work with compressed `.PSSG.gz` files.

## What You Need
- .NET Framework 4.8 or later

## to Use
1. **Extracted Files**:
   - Extracted G1T files will be saved using the names found inside the `.PSSG` file. If there are any invalid characters in the filenames, they will be replaced with underscores (`_`).

## Important Details
- **Main Logic**: The tool reads `.PSSG` files, finds specific parts of the data, and saves them as individual G1T files.

## Error Handling
- If a filename has invalid characters, they will be replaced to avoid errors.
- If an error occurs, the tool will display a message and continue to process other files.

## Notes
- This tool does not support `.PSSG.gz` compressed files. Please use Ego PSSG Editor instead.

## Example
- **Input**: A folder with files like `example.PSSG`.
- **Output**: Extracted files like `texture1.g1t`, `model_data.g1t`, etc.

## Disclaimer
This tool is provided "as-is" without any guarantees. Please use it responsibly and follow all applicable laws and terms.

