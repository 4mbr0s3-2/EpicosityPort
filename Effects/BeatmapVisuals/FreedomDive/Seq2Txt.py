from PIL import Image

file = open("output.txt", "w")

for currentFrame in range(0, int(input("End frame: "))):
    print("Optimizing frame " + str(currentFrame))
    im = Image.open("seq/" + str(currentFrame).zfill(4) + ".png") # Can be many different formats.
    pix = im.load()
    
    for x in range(im.size[0]):
        for y in range(im.size[1]):
            if pix[x, y][0] != 0:
                file.write(str(im.size[1] - y))
                file.write(',')
                break
    file.write('\n')
        
file.close()
