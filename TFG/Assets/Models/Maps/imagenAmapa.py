from PIL import Image

pixelNegro = (0, 0, 0, 255);
pixelBlanco = (255, 255, 255, 255);
pixelAmarillo = (255, 255, 0, 255);
pixelVerde = (0, 255, 0, 255);
pixelAzul = (0, 0, 255, 255);
pixelRojo = (255, 0, 0, 255);


im = Image.open("map-tutorial2.png")

pix = im.load()

tamX = im.size[0]
tamY = im.size[1]

print('['),
for y in range(0, tamY):
	print('['),
	for x in range(0, tamX):
		if cmp(pix[x,y], pixelNegro) == 0:
			print('0'),

		elif cmp(pix[x,y], pixelBlanco) == 0:
			print('1'),

		elif cmp(pix[x,y], pixelAmarillo) == 0:
			print('2'),

		elif cmp(pix[x,y], pixelVerde) == 0:
			print('3'),

		elif cmp(pix[x,y], pixelAzul) == 0:
			print('4'),

		elif cmp(pix[x,y], pixelRojo) == 0:
			print('5'),

		if x != tamX-1:
			print(','),

	print('],'),
	

	print('\n'),;
		
print(']'),