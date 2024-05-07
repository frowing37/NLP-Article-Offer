import os

def read_txt_files(folder_path):
    file_contents = []
    
    # Klasördeki tüm dosyaları gez
    for file_name in os.listdir(folder_path):
        if file_name.endswith('.txt'):  # Sadece .txt uzantılı dosyaları al
            file_path = os.path.join(folder_path, file_name)
            # Dosyayı oku
            with open(file_path, 'r') as file:
                content = file.read()
                file_contents.append(content)
    
    return file_contents

# Örnek kullanım
folder_path = "./filtered_texts"
contents = read_txt_files(folder_path)
#print(contents)
