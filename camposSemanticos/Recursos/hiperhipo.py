'''import nltk
from nltk.corpus import wordnet

def get_hypernyms_hyponyms(word, lang='spa', hypernyms_hyponyms=None):
    if hypernyms_hyponyms is None:
        hypernyms_hyponyms = set()
    synsets = wordnet.synsets(word, lang=lang)
    for syn in synsets:
        for hypernym in syn.hypernyms():
            for lemma in hypernym.lemmas(lang=lang):
                hypernyms_hyponyms.add(lemma.name())
            hypernyms_hyponyms.update(get_hypernyms_hyponyms(hypernym.name(), lang, hypernyms_hyponyms))
        for hyponym in syn.hyponyms():
            for lemma in hyponym.lemmas(lang=lang):
                hypernyms_hyponyms.add(lemma.name())
            hypernyms_hyponyms.update(get_hypernyms_hyponyms(hyponym.name(), lang, hypernyms_hyponyms))
    return hypernyms_hyponyms
'''

import sys
import nltk
from nltk.corpus import wordnet

def get_hypernyms_hyponyms(word, lang='spa', hypernyms_hyponyms=None):
    if hypernyms_hyponyms is None:
        hypernyms_hyponyms = set()
    synsets = wordnet.synsets(word, lang=lang)
    for syn in synsets:
        for hypernym in syn.hypernyms():
            for lemma in hypernym.lemmas(lang=lang):
                hypernyms_hyponyms.add(lemma.name())
            hypernyms_hyponyms.update(get_hypernyms_hyponyms(hypernym.name(), lang, hypernyms_hyponyms))
        for hyponym in syn.hyponyms():
            for lemma in hyponym.lemmas(lang=lang):
                hypernyms_hyponyms.add(lemma.name())
            hypernyms_hyponyms.update(get_hypernyms_hyponyms(hyponym.name(), lang, hypernyms_hyponyms))
    return hypernyms_hyponyms

if __name__ == "__main__":
    word = sys.argv[1]
    hypernyms_hyponyms = get_hypernyms_hyponyms(word, lang='spa')
    result = ", ".join(hypernyms_hyponyms)
    print(result)