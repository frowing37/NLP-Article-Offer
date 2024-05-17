import torch
from transformers import AutoTokenizer, AutoModel

class DataStem2:
 
  def makeStem(self, contents):
     tokenizer = AutoTokenizer.from_pretrained('allenai/scibert_scivocab_uncased')
     model = AutoModel.from_pretrained('allenai/scibert_scivocab_uncased')
     text = "Bilimsel metinleri vektörleştirmek için SciBERT kullanıyoruz."
     inputs = tokenizer(text, return_tensors='pt')
     with torch.no_grad():
         outputs = model(**inputs)
     last_hidden_states = outputs.last_hidden_state
     mean_embedding = last_hidden_states.mean(dim=1).squeeze().numpy()

     return(mean_embedding.tolist())
