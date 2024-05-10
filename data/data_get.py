import os

def read_txt_files(folder_path):
    file_contents = []
    
    # Klasör yolunun varlığını kontrol et
    if not os.path.exists(folder_path):
        print("Hata: Belirtilen klasör bulunamadı.")
        return None
    
    # Klasördeki tüm dosyaları gez
    for file_name in os.listdir(folder_path):
        if file_name.endswith('.txt'):  # Sadece .txt uzantılı dosyaları al
            file_path = os.path.join(folder_path, file_name)
            try:
                # Dosyayı oku
                with open(file_path, 'r', encoding='utf-8') as file:
                    content = file.read()
                    file_contents.append(content)
            except Exception as e:
                print(f"Hata: {file_path} dosyası okunamadı. Hata: {e}")
    
    return file_contents

# Örnek kullanım
folder_path = "../filtered_texts"
contents = read_txt_files(folder_path)
print(contents)

