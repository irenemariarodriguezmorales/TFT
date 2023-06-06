'''import sys
import nltk
from nltk.corpus import wordnet

def get_synonyms(word):
    synonyms = []
    for syn in wordnet.synsets(word, lang='spa'):
        for lemma in syn.lemmas(lang='spa'):
            synonyms.append(lemma.name())
    result = ", ".join(set(synonyms))  
    return result
'''

import sys
import nltk
from nltk.corpus import wordnet

if __name__ == "__main__":
    word = sys.argv[1]
    synonyms = []
    for syn in wordnet.synsets(word, lang='spa'):
        for lemma in syn.lemmas(lang='spa'):
            synonyms.append(lemma.name())
    result = ", ".join(set(synonyms))
    print(result)
