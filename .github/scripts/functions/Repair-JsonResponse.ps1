function Repair-JsonResponse {
    <#
    .SYNOPSIS
        Repairs invalid JSON escape sequences in AI model responses.
    
    .DESCRIPTION
        AI models (like GPT-4) sometimes generate invalid JSON escape sequences 
        such as \s, \d, \w (regex patterns) or accidental backslashes before letters.
        
        Valid JSON escape sequences are ONLY: \" \\ \/ \b \f \n \r \t \uXXXX
        
        This function fixes invalid escapes by converting them to valid JSON:
        - \s becomes \\s (escaped backslash)
        - \d becomes \\d (escaped backslash)
        - etc.
    
    .PARAMETER JsonString
        The JSON string to repair.
    
    .EXAMPLE
        $fixed = Repair-JsonResponse -JsonString '{"text": "keyboard\shortcuts"}'
        # Returns: '{"text": "keyboard\\shortcuts"}'
    
    .NOTES
        This function should be called before ConvertFrom-Json to prevent
        "Bad JSON escape sequence" errors.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [string]$JsonString
    )

    if ([string]::IsNullOrEmpty($JsonString)) {
        return $JsonString
    }

    # Valid JSON escape characters (after the backslash)
    # " \ / b f n r t u (for unicode \uXXXX)
    # Everything else is invalid and needs the backslash escaped
    
    # Strategy: Find all backslash sequences and escape invalid ones
    # We need to be careful not to double-escape already valid sequences
    
    # Use a regex to find backslash followed by any character that's NOT a valid escape
    # Valid escapes: " \ / b f n r t u
    # Invalid: anything else (s, d, w, a, e, c, g, h, i, j, k, l, m, o, p, q, v, x, y, z, etc.)
    
    $result = [System.Text.StringBuilder]::new($JsonString.Length * 2)
    $i = 0
    
    while ($i -lt $JsonString.Length) {
        $char = $JsonString[$i]
        
        if ($char -eq '\' -and ($i + 1) -lt $JsonString.Length) {
            $nextChar = $JsonString[$i + 1]
            
            # Check if the next character is a valid JSON escape character
            switch ($nextChar) {
                '"' { 
                    # Valid: \"
                    $null = $result.Append('\')
                    $null = $result.Append('"')
                    $i += 2
                }
                '\' { 
                    # Valid: \\
                    $null = $result.Append('\\')
                    $i += 2
                }
                '/' { 
                    # Valid: \/
                    $null = $result.Append('\/')
                    $i += 2
                }
                'b' { 
                    # Valid: \b (backspace)
                    $null = $result.Append('\b')
                    $i += 2
                }
                'f' { 
                    # Valid: \f (form feed)
                    $null = $result.Append('\f')
                    $i += 2
                }
                'n' { 
                    # Valid: \n (newline)
                    $null = $result.Append('\n')
                    $i += 2
                }
                'r' { 
                    # Valid: \r (carriage return)
                    $null = $result.Append('\r')
                    $i += 2
                }
                't' { 
                    # Valid: \t (tab)
                    $null = $result.Append('\t')
                    $i += 2
                }
                'u' { 
                    # Valid: \uXXXX (unicode) - need to check for 4 hex digits
                    if (($i + 5) -lt $JsonString.Length) {
                        $hex = $JsonString.Substring($i + 2, 4)
                        if ($hex -match '^[0-9a-fA-F]{4}$') {
                            # Valid unicode escape
                            $null = $result.Append('\u')
                            $null = $result.Append($hex)
                            $i += 6
                        }
                        else {
                            # Invalid: \u not followed by 4 hex digits - escape the backslash
                            $null = $result.Append('\\')
                            $null = $result.Append('u')
                            $i += 2
                        }
                    }
                    else {
                        # Not enough characters for \uXXXX - escape the backslash
                        $null = $result.Append('\\')
                        $null = $result.Append('u')
                        $i += 2
                    }
                }
                default {
                    # Invalid escape sequence - escape the backslash to make it valid
                    # \s becomes \\s, \d becomes \\d, etc.
                    $null = $result.Append('\\')
                    $null = $result.Append($nextChar)
                    $i += 2
                }
            }
        }
        else {
            # Regular character, just append
            $null = $result.Append($char)
            $i++
        }
    }
    
    return $result.ToString()
}
