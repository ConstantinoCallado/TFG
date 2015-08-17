import bpy

def instanciarConNombre(nombre):
    bpy.data.objects[nombre].select = True
    newObj = bpy.ops.object.duplicate()

    bpy.data.objects[nombre].select = False

def instanciarEnPosicion(nombre, posicionX, posicionY):
    instanciarConNombre(nombre)
    
    for obj in bpy.context.selected_objects:
        obj.location = (posicionX,posicionY,0)
    
    bpy.ops.object.select_all()
    


instanciarEnPosicion('Relleno', 5, 5)
instanciarEnPosicion('Relleno', 6, 5)
instanciarEnPosicion('Relleno', 7, 5)

#PALUEGO: bpy.ops.object.join()