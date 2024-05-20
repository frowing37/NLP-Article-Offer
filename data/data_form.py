import fasttext
import string
import numpy as np
import nltk
from nltk.tokenize import word_tokenize
from nltk.corpus import stopwords
from nltk.stem import PorterStemmer

class DataStem:
    def __init__(self, model_path):
        self.fasttext_model = fasttext.load_model(model_path)

    def makeStem(self, contents):
        #nltk.download('punkt')
        #nltk.download('stopwords')

        all_stems = []
        
        for text in contents:
            if not text.strip():
              continue
            text_lower = text.lower()
            text_no_punctuation = text_lower.translate(str.maketrans("", "", string.punctuation))
            words = word_tokenize(text_no_punctuation, language='english')
            stop_words = set(stopwords.words('english'))
            filtered_words = [word for word in words if word not in stop_words and len(word) > 1]
            stemmer = PorterStemmer()
            stemmed_words = [stemmer.stem(word) for word in filtered_words]
            
            vectors = []
            for token in stemmed_words:
                vector = self.fasttext_model.get_word_vector(token)
                if vector is not None:
                    vectors.append(vector)
            
            if vectors:
                # Vektörlerin ortalamasını al
                avg_vector = np.mean(vectors, axis=0)
                all_stems.append(avg_vector.tolist())  # JSON'a dönüştürülebilir hale getir
                
        return all_stems

