import os

def fix_newlines_in_dir(directory):
    for root, _, files in os.walk(directory):
        for file in files:
            if file.endswith(".cshtml"):
                path = os.path.join(root, file)
                with open(path, "r", encoding="utf-8") as f:
                    content = f.read()
                
                if "\\n" in content:
                    # Replace literal "\n" strings with actual newlines
                    new_content = content.replace("\\n", "\n")
                    with open(path, "w", encoding="utf-8") as f:
                        f.write(new_content)
                    print(f"Fixed newlines in {path}")

fix_newlines_in_dir("Views")
