'''import nltk
from nltk.corpus import wordnet

def get_antonyms(word):
    antonyms = []
    for syn in wordnet.synsets(word, lang='spa'):
        for lemma in syn.lemmas(lang='spa'):
            if lemma.antonyms():
                antonyms.append(lemma.antonyms()[0].name())
    result = ", ".join(set(antonyms))
    return result'''

import sys
import nltk
from nltk.corpus import wordnet

if __name__ == "__main__":
    word = sys.argv[1]
    antonyms = []
    for syn in wordnet.synsets(word, lang='spa'):
        for lemma in syn.lemmas(lang='spa'):
            if lemma.antonyms():
                antonyms.append(lemma.antonyms()[0].name())
    result = ", ".join(set(antonyms))
    print(result)

