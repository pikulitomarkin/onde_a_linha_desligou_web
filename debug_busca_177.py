import gpxpy
import os
from pathlib import Path

# Simula a busca da torre 177
codigo_torre = "177"
df_key = "lna_assis"
gpx_file = "assisc1.gpx"

# Ajusta o código da torre (simulando a função ajustar_codigo_torre)
numero_torre = codigo_torre.strip()
if df_key == "lna_assis":
    numero_torre = numero_torre.zfill(3)  # Força 3 dígitos com zeros à esquerda

print(f"Código original: '{codigo_torre}'")
print(f"Código ajustado: '{numero_torre}'")

# Carrega o GPX
gpx_path = Path("static/resources") / gpx_file
print(f"Carregando arquivo: {gpx_path}")

with open(gpx_path, "r", encoding="utf-8") as gpx_file_obj:
    gpx = gpxpy.parse(gpx_file_obj)
    
    print(f"Total de waypoints: {len(gpx.waypoints)}")
    
    # Procura todas as torres com 177
    torres_177 = []
    for waypoint in gpx.waypoints:
        nome_wp = str(waypoint.name).strip()
        if "177" in nome_wp:
            torres_177.append(nome_wp)
    
    print(f"Torres com '177' encontradas: {torres_177}")
    
    # Testa a lógica atual
    print("\nTestando lógica atual:")
    for waypoint in gpx.waypoints:
        nome_wp = str(waypoint.name).strip()
        
        # Testa os formatos da linha lna_assis
        if nome_wp == f"7350TO{numero_torre}" or nome_wp == f"7330TO{numero_torre}":
            print(f"ENCONTROU com prefixo: {nome_wp}")
            break
        elif nome_wp == numero_torre:
            print(f"ENCONTROU direto: {nome_wp}")
            break
    else:
        print("NÃO ENCONTROU com a lógica atual")
        
    # Testa busca por nome simples "177"
    print("\nTestando busca direta por '177':")
    for waypoint in gpx.waypoints:
        nome_wp = str(waypoint.name).strip()
        if nome_wp == "177":
            print(f"ENCONTROU busca direta: {nome_wp} - Lat: {waypoint.latitude}, Lon: {waypoint.longitude}")
            break
    else:
        print("NÃO ENCONTROU busca direta")
