import gpxpy
import os
from pathlib import Path

# Simula a busca da torre 177 com a nova lógica
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
            torres_177.append((nome_wp, waypoint.latitude, waypoint.longitude))
    
    print(f"Torres com '177' encontradas: {len(torres_177)}")
    for nome, lat, lon in torres_177:
        print(f"  - {nome} (Lat: {lat}, Lon: {lon})")
    
    # Testa a nova lógica (apenas busca direta)
    print(f"\nTestando nova lógica - busca direta por '{numero_torre}':")
    for waypoint in gpx.waypoints:
        nome_wp = str(waypoint.name).strip()
        
        # Nova lógica: apenas busca direta para lna_assis
        if "assisc1" in gpx_file and nome_wp == numero_torre:
            print(f"ENCONTROU busca direta: {nome_wp} - Lat: {waypoint.latitude}, Lon: {waypoint.longitude}")
            break
    else:
        print("NÃO ENCONTROU com a nova lógica")
        
    print(f"\nTorres que serão IGNORADAS (com prefixo):")
    for nome, lat, lon in torres_177:
        if nome != numero_torre:
            print(f"  - {nome} (IGNORADA)")
