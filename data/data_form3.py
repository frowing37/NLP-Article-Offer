import numpy as np
from sklearn.metrics.pairwise import cosine_similarity
from sklearn.metrics import precision_score, recall_score

def calculate_precision_recall(true_labels, predicted_labels):
    precision = precision_score(true_labels, predicted_labels)
    recall = recall_score(true_labels, predicted_labels)
    return precision, recall

def calculate_similarity(vectors):
    return cosine_similarity(vectors)

def predict_labels(similarity_matrix, threshold):
    return [1 if sim >= threshold else 0 for sim in similarity_matrix]

# Örnek vektörler (dinamik boyutlu vektörler)
num_objects = 10
vector_dim = 300  # Vektör boyutu
scibert_vectors = np.random.rand(num_objects, vector_dim)
fastmodel_vectors = np.random.rand(num_objects, vector_dim)

# Eşik değeri belirleyin (örneğin 0.8)
threshold = 0.8

# Gerçek etiketler (örneğin, ground truth etiketleri)
true_labels = [1, 0, 1, 0, 1, 0, 1, 0, 1, 0]  # Gerçek benzerlikler

# Benzerlik matrislerini hesapla
scibert_similarity = calculate_similarity(scibert_vectors)
fastmodel_similarity = calculate_similarity(fastmodel_vectors)

# Tahmin etiketlerini oluştur
predicted_labels_scibert = predict_labels(scibert_similarity[0], threshold)
predicted_labels_fastmodel = predict_labels(fastmodel_similarity[0], threshold)

# Precision ve Recall hesapla
precision_scibert, recall_scibert = calculate_precision_recall(true_labels, predicted_labels_scibert)
precision_fastmodel, recall_fastmodel = calculate_precision_recall(true_labels, predicted_labels_fastmodel)

print(f"SciBERT Precision: {precision_scibert}, Recall: {recall_scibert}")
print(f"FastModel Precision: {precision_fastmodel}, Recall: {recall_fastmodel}")
