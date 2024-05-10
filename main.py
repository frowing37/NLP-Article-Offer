import nltk
from nltk.tokenize import word_tokenize
from nltk.corpus import stopwords
from nltk.stem import PorterStemmer
from snowballstemmer import TurkishStemmer
import fasttext
import string

# Metin
text = "!fucked your, momming?"

# Türkçe için stopwords'ları yükleme
#nltk.download('stopwords')

# Türkçe için dil modelini yükleme
#nltk.download('punkt')

# Metni küçük harflere dönüştürme
text_lower = text.lower()

# Noktalama işaretlerini kaldırma
text_no_punctuation = text_lower.translate(str.maketrans("", "", string.punctuation))

# Metni kelimelere ayırma
words = word_tokenize(text_no_punctuation, language='turkish')

# Türkçe stopwords'leri yükleme
stop_words = set(stopwords.words('turkish'))

# Stopwords'leri ve tek harfli kelimeleri temizleme
filtered_words = [word for word in words if word not in stop_words and len(word) > 1]

# Kelime köklerini bulma (Stemming)
stemmer = PorterStemmer()
stemmed_words = [stemmer.stem(word) for word in filtered_words]

# fasttext modellerini yüklüyor
fasttext_model = fasttext.load_model('/Users/frowing/Downloads/cc.en.300.bin')

# fasttext modeliyle vektör karşılaştırmasını yapıyor
vectors = [fasttext_model.get_word_vector(token) for token in stemmed_words]

vectors_list = []
for vector in vectors:
    if vector is not None:
        vectors_list.append(vector)


# Sonuçları görüntüleme
print("Temizlenmiş kelimeler:", filtered_words)
print("Kökleri bulunmuş kelimeler:", stemmed_words)
print("Vektörler:",vectors_list)
