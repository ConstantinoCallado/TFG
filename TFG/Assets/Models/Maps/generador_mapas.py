import bpy

def instanciarConNombre(nombre):
    bpy.data.objects[nombre].select = True
    newObj = bpy.ops.object.duplicate()

    bpy.data.objects[nombre].select = False

def instanciarEnPosicion(nombre, posicionX, posicionY):
    instanciarConNombre(nombre)
    
    for obj in bpy.context.selected_objects:
        obj.location = (posicionX,posicionY,0)
    
    obj.layers[0] = False
    obj.layers[1] = True
    bpy.ops.object.select_all()

#AVISO! Las rotaciones hay que definirlas en radianes:
# 0 rad = 0º
# 0.5 PI rad = 90º
# PI rad = 180º
# 1.5 pi rad = 270º
def instanciarEnPosicionConRot(nombre, posicionX, posicionY, rot):
    instanciarConNombre(nombre)
    
    for obj in bpy.context.selected_objects:
        obj.location = (posicionX,posicionY,0)
        obj.rotation_euler = (0, 0, rot)
        
    obj.layers[0] = False
    obj.layers[1] = True
    bpy.ops.object.select_all()


matrizMapa = [ [ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ], 
[ 0 , 0 , 3 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 1 , 1 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 3 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 2 , 0 , 0 , 0 , 2 , 1 , 1 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 1 , 1 , 2 , 0 , 0 , 0 , 2 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 4 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 4 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 1 , 2 , 1 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 1 , 2 , 1 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 2 , 0 , 0 , 2 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 1 , 1 , 1 , 2 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 1 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 2 , 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 , 2 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 , 5 , 1 , 1 , 5 , 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 ], 
[ 1 , 1 , 2 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 2 , 1 , 1 ], 
[ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 5 , 1 , 1 , 5 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 ], 
[ 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 ], 
[ 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 ], 
[ 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 ], 
[ 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 4 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 4 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 2 , 1 , 1 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 1 , 1 , 2 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 ], 
[ 0 , 0 , 3 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 3 , 0 , 0 ], 
[ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ]
]






tamY = len(matrizMapa);
tamX = len(matrizMapa[0]);

def isPosicionAndable(x, y):
    if x < 0 or x==tamX or y<0 or y==tamY or matrizMapa[y][x] == 0:
        return False
    else:
        return True

def contarHuecosAdyacentes(x, y):
    numeroHuecos = 0
    
    if isPosicionAndable(x+1, y):
        numeroHuecos += 1
    
    if isPosicionAndable(x+1, y+1):
        numeroHuecos += 1
        
    if isPosicionAndable(x, y+1):
        numeroHuecos += 1
        
    if isPosicionAndable(x-1, y+1):
        numeroHuecos += 1
    
    if isPosicionAndable(x-1, y):
        numeroHuecos += 1
    
    if isPosicionAndable(x-1, y-1):
        numeroHuecos += 1
    
    if isPosicionAndable(x, y-1):
        numeroHuecos += 1
    
    if isPosicionAndable(x+1, y-1):
        numeroHuecos += 1
    
    return numeroHuecos

for y in range(0, tamY):
    for x in range(0, tamX):
        # Si estamos en una celda donde hay obstáculo...
        if not isPosicionAndable(x, y):
            huecosAdyacentes = contarHuecosAdyacentes(x,y)
            
            # Con 0 huecos adyacentes instanciamos el techo
            if huecosAdyacentes == 0:
                instanciarEnPosicion('Relleno', x, y)
            
            # Con un solo hueco adyacente instanciamos un rincon
            elif huecosAdyacentes == 1:
                if isPosicionAndable(x+1, y+1):
                    instanciarEnPosicionConRot('Rincon', x, y, 3.14159265)
                    
                elif isPosicionAndable(x-1, y-1):    
                    instanciarEnPosicionConRot('Rincon', x, y, 0)
                    
                elif isPosicionAndable(x+1, y-1):
                    instanciarEnPosicionConRot('Rincon', x, y, 1.5707963311)
                    
                elif isPosicionAndable(x-1, y+1):    
                    instanciarEnPosicionConRot('Rincon', x, y, 4.71238898)
            
            # Con cinco huecos adyacentes instanciamos una esquina
            elif huecosAdyacentes == 5:
                if not isPosicionAndable(x+1, y+1):
                    instanciarEnPosicionConRot('Esquina', x, y, 0)
                    
                elif not isPosicionAndable(x-1, y-1):    
                    instanciarEnPosicionConRot('Esquina', x, y, 3.14159265)
                    
                elif not isPosicionAndable(x+1, y-1):
                    instanciarEnPosicionConRot('Esquina', x, y, 4.71238898)
                    
                elif not isPosicionAndable(x-1, y+1):    
                    instanciarEnPosicionConRot('Esquina', x, y, 1.57079633)

            # Sino... instanciamos un muro
            else
                if isPosicionAndable(x+1, y):
                    instanciarEnPosicionConRot('Muro', x, y, 3.14159265)
                    
                elif isPosicionAndable(x-1, y):    
                    instanciarEnPosicionConRot('Muro', x, y, 0)
                    
                elif isPosicionAndable(x, y+1):
                    instanciarEnPosicionConRot('Muro', x, y, 4.71238898)
                    
                elif isPosicionAndable(x, y-1):    
                    instanciarEnPosicionConRot('Muro', x, y, 1.57079633)