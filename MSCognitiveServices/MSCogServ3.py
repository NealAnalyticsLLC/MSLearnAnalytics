import requests
import numpy as np
import cv2
from PIL import Image
import matplotlib.pyplot as plt


#Create an OCR, VisFeatures Class class

class CogServ:
    def __init__(self):

        #Dictionary of paramereters and API URLs
        self.__CS = dict()
        self.__CS['Analyze'] = {'params':{ 'visualFeatures' :
                             'Color, Categories, Tags, Description, ImageType, Adult, Faces','details' : 'Celebrities'},
                             'url': 'https://westus.api.cognitive.microsoft.com/vision/v1.0/analyze'}
        self.__CS['OCR'] = {'params':{'detectOrientation': 'true', 'language': 'unk'},
                          'url':'https://westus.api.cognitive.microsoft.com/vision/v1.0/ocr?language=unk&detectOrientation =true'}
        self.__CS['Face'] = {'params': None,
                          'url':'https://westus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true'}
        self.__CS['Emotion'] = {'params': None,
                          'url':'https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize'}


        self.img = None
        self.__data = None
        self.__json = None
        self.source = None
        self.url = None

    def get_Source(self, source, url):

        '''
        Retrieve an image from either the web or disk
        INPUT:
        source  url or path to file as string
        url is boolean True is from url

        OUTPUT:
        numpy array of the image

        sets json and data for CompVis method
        sets headers for CompVis method (url or disk)
        '''

        self.source = source
        self.url = url

        if url:
            arr = np.asarray(bytearray(requests.get(source).content), dtype=np.uint8)
            self.img = cv2.cvtColor(cv2.imdecode( arr, -1 ), cv2.COLOR_BGR2RGB)
        else:
            self.img = Image.open(source)
        return self.img


    def set_headers(self, key):
        self.__headers = dict()
        self.__headers['Ocp-Apim-Subscription-Key'] = key
        self.__headers['Content-Type'] = None

        if self.url:
            self.__headers['Content-Type'] = 'application/json'
            self.__json = { 'url': self.source }
            self.__data = None
        else:
            self.__headers['Content-Type'] = 'application/octet-stream'
            with open(self.source, 'rb' ) as f:
                self.__data = f.read()
            self.__json = None
        return


    def API(self, API, key):
        '''
        INPUT: API is which API to call and key
        depending on the API, the appropriate API URL, headers, and params are selectedl from CS dict
        json and data are the image data set with the get image method
        OUTPUT: result from API
        '''

        #sets headers with appropriate key and file info
        self.set_headers(key)

        while True:

            response = requests.request( 'post',
                                        self.__CS[API]['url'],
                                        json = self.__json,
                                        data = self.__data,
                                        headers = self.__headers,
                                        params = self.__CS[API]['params'])
            if response.status_code == 429:
                print( "Message: %s" % ( response.json()['error']['message'] ) )
                if retries <= _maxNumRetries:
                    time.sleep(1)
                    retries += 1
                    continue
                else:
                    print( 'Error: failed after retrying!' )
                    break
            elif response.status_code == 200 or response.status_code == 201:
                if 'content-length' in response.headers and int(response.headers['content-length']) == 0:
                    result = None
                elif 'content-type' in response.headers and isinstance(response.headers['content-type'], str):
                    if 'application/json' in response.headers['content-type'].lower():
                        result = response.json() if response.content else None
                    elif 'image' in response.headers['content-type'].lower():
                        result = response.content
            else:
                print( "Error code: %d" % ( response.status_code ) )
                print( "Message: %s" % ( response.json()['error']['message'] ) )
            break
        #self.__set_results(result, API)
        return result


    def get_OCR(self, key):
        result = self.API('OCR', key)
        self.OCR = self.__OCR()
        words = []
        for r in result['regions']:
            for d in r['lines']:
                for w in d['words']:
                    words.append(w['text'])
        self.OCR.words = ' '.join(words)
        self.OCR.result = result
        #print self.words
        return

    def get_Analyze(self, key):
        result = self.API('Analyze', key)
        self.Analyze = self.__Analyze(self.img)
        self.Analyze.result = result
        self.Analyze.categories = result['categories'][0]['name'], result['categories'][0]['score']
        self.Analyze.tags = result['description']['tags']
        self.Analyze.caption = result['description']['captions'][0]['text'], result['description']['captions'][0]['confidence']
        return

    def get_Face(self, key):
        result = self.API('Face', key)
        self.Face = self.__Face()
        self.Face.result = result
        return

    def get_Emotion(self, key):
        result = self.API('Emotion', key)
        self.Emotion = self.__Emotion()
        self.Emotion.result = result
        return

    class __OCR:
        def __init__(self):
            self.result = None
            self.words = None
    class __Analyze():
        #takes self.img as input
        #this allows the faces function to operate on the image
        def __init__(self, img):
            self.result = None
            self.categories = None
            self.tags = None
            self.caption = None
            self.__img = img

        def faces(self):
            #displays the faces found in the image
            face = self.result['faces']
            for f in face:
                face = f['faceRectangle']
                plt.imshow(self.__img[face['top']:(face['top'] + face['height']),
                                      face['left']:(face['left'] + face['width'])])
                plt.show()

    class __Face:
        def __init__(self):
            self.result = None

    class __Emotion:
        def __init__(self):
            self.result = None

class images:
    def __init__(self):
        self.urls = ['https://scontent.fsnc1-1.fna.fbcdn.net/v/t1.0-9/12994305_10208933675241355_5831435046410234566_n.jpg?oh=4dad63d1843149aed3a512f1aaaca338&oe=594B9129',
         'http://www.coca-colaproductfacts.com/content/dam/productfacts/us/productDetails/ProductImages/PDP_Coca-Cola_HFCS_glass_8oz.png',
         'https://cdn.shopify.com/s/files/1/0669/0045/products/Regular_Coke_Cans.png?v=1417745229',
         'http://seamonsterlounge.com/images/igallery/resized/1-100/seamo_front_sign_3-16-300-300-80-rd-0-0-0.jpg',
         'http://www.gannett-cdn.com/-mm-/cf04580d6c98e560864fbe8e797b9ec3c8d0ba66/c=0-110-3803-2259/local/-/media/2016/09/04/Salem/Salem/636085460364461412-Angels-Mariners-Baseb-Kirk.jpg',
         'http://www.logotransfers.com/bmz_cache/4/42fe2dc2791c9ffe88c009a3d98d6941.image.280x181.png',
         'http://thedolphinlmc.com/wp-content/uploads/2016/02/TRUMP-make-america-great-again-WHITE_5936.jpg',
         'https://upload.wikimedia.org/wikipedia/commons/thumb/d/d5/I_Love_New_York.svg/2000px-I_Love_New_York.svg.png',
         'http://i2.kym-cdn.com/entries/icons/original/000/018/642/trump-hat.jpg',
         'https://sidoxia.files.wordpress.com/2012/02/ny-times.jpg',
         'https://pbs.twimg.com/profile_images/1877885638/ss_mark.jpg',
         'https://c2.staticflickr.com/4/3423/3388087139_458c9f2ccb_z.jpg?zz=1',
         'https://i.ytimg.com/vi/kbxtYqA6ypM/maxresdefault.jpg',
         'http://image.shutterstock.com/z/stock-photo-russian-alphabet-with-funny-animals-145433797.jpg']
