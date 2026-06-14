import os
import re

def process_file(source_path, target_path, replacements):
    if not os.path.exists(source_path):
        print(f"Source file not found: {source_path}")
        return
        
    with open(source_path, 'r', encoding='utf-8') as f:
        content = f.read()
        
    for old, new in replacements:
        content = content.replace(old, new)
        
    os.makedirs(os.path.dirname(target_path), exist_ok=True)
    with open(target_path, 'w', encoding='utf-8') as f:
        f.write(content)
    print(f"Created: {target_path}")

def generate_crud(template_model, target_model, target_model_plural):
    print(f"Generating CRUD for {target_model} from {template_model}...")
    
    # Define replacements
    # Need to be careful with casing
    template_plural = template_model + "s"
    replacements = [
        (template_plural, target_model_plural), # e.g. Rooms -> Departments
        (template_model, target_model),         # e.g. Room -> Department
        (template_plural.lower(), target_model_plural.lower()),
        (template_model.lower(), target_model.lower()),
    ]
    
    # 1. Controller
    ctrl_src = f"Controllers/{template_plural}Controller.cs"
    ctrl_target = f"Controllers/{target_model_plural}Controller.cs"
    process_file(ctrl_src, ctrl_target, replacements)
    
    # 2. Views
    views_dir_src = f"Views/{template_plural}"
    views_dir_target = f"Views/{target_model_plural}"
    
    view_files = ["Create.cshtml", "Delete.cshtml", "Details.cshtml", "Edit.cshtml", "Index.cshtml"]
    
    for vf in view_files:
        src = os.path.join(views_dir_src, vf)
        target = os.path.join(views_dir_target, vf)
        process_file(src, target, replacements)

# Run generation
# For models with HotelId
generate_crud("Room", "Department", "Departments")
generate_crud("Room", "InventoryItem", "InventoryItems")
generate_crud("Room", "PurchaseOrder", "PurchaseOrders")
generate_crud("Room", "Expense", "Expenses")

# For models without HotelId
generate_crud("Hotel", "Supplier", "Suppliers")

print("Done generating.")
