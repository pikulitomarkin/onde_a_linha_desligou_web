import gpxpy
import os

# Carrega o arquivo assisc1.gpx
gpx_file = 'static/resources/assisc1.gpx'
if os.path.exists(gpx_file):
    with open(gpx_file, 'r', encoding='utf-8') as f:
        gpx = gpxpy.parse(f)
    
    print(f'Analisando arquivo: {gpx_file}')
    print(f'Total de waypoints: {len(gpx.waypoints)}')
    
    # Procura por torres com '177' no nome
    torres_177 = []
    for waypoint in gpx.waypoints:
        if '177' in waypoint.name:
            torres_177.append(waypoint.name)
    
    print(f'\nTorres com "177" no nome: {len(torres_177)}')
    for torre in torres_177:
        print(f'  - {torre}')
    
    # Procura por variações de 177
    variacao_177 = []
    for waypoint in gpx.waypoints:
        name = waypoint.name.upper()
        if any(x in name for x in ['177', 'TO177', 'V177', 'T177']):
            variacao_177.append(waypoint.name)
    
    print(f'\nTorres com variações de "177": {len(variacao_177)}')
    for torre in variacao_177:
        print(f'  - {torre}')
    
    # Mostra primeiros e últimos waypoints para entender o padrão
    print(f'\nPrimeiros 10 waypoints:')
    for i, waypoint in enumerate(gpx.waypoints[:10]):
        print(f'{i+1:2d}. Nome: "{waypoint.name}"')
        
    print(f'\nÚltimos 10 waypoints:')
    for i, waypoint in enumerate(gpx.waypoints[-10:], len(gpx.waypoints)-9):
        print(f'{i:2d}. Nome: "{waypoint.name}"')
        
else:
    print(f'Arquivo {gpx_file} não encontrado!')
