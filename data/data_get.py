import os
import numpy as np
from sklearn.metrics.pairwise import cosine_similarity

from model.articleModel import articleModel


class DataRead:
    def readFilesName(self):
        folder_path = "../filtered_texts"
        file_names = []

        for file_name in os.listdir(folder_path):
            file_names.append(file_name)

        return file_names

    def readAllTxt(self):
        folder_path = "../filtered_texts"
        file_contents = []
        
        if not os.path.exists(folder_path):
            raise FileNotFoundError("Belirtilen klasör bulunamadı.")
        
        for file_name in os.listdir(folder_path):
            if file_name.endswith('.txt'):
                file_path = os.path.join(folder_path, file_name)
                try:
                    with open(file_path, 'r', encoding='utf-8') as file:
                        content = file.read()
                        articleObject = articleModel(file_name,content)
                        file_contents.append(articleObject)
                except Exception as e:
                    print(f"Hata: {file_path} dosyası okunamadı. Hata: {e}")
                    continue
        
        return file_contents

    def readTxtByName(self, file_name):
        folder_path = "../filtered_texts"
        file_contents = []
        
        if not os.path.exists(folder_path):
            raise FileNotFoundError("Belirtilen klasör bulunamadı.")
        
        for file in os.listdir(folder_path):
            if file.endswith('.txt') and file == file_name:
                file_path = os.path.join(folder_path, file)
                try:
                    with open(file_path, 'r', encoding='utf-8') as file:
                        content = file.read()
                        file_contents.append(content)
                except Exception as e:
                    print(f"Hata: {file_path} dosyası okunamadı. Hata: {e}")
                    # Hata durumunda devam et
                    continue
        
        return file_contents

    def cosine_similarity(self, vector1, vector2):
     vector1_array = np.array(vector1)
     vector2_array = np.array(vector2)
     similarity = cosine_similarity(vector1_array.reshape(1, -1), vector2_array.reshape(1, -1))[0, 0]
     return similarity
