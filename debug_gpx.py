import gpxpy
from pathlib import Path

# Carrega o GPX da linha lna_assis
gpx_path = Path('static/resources/lna_assis.gpx')
if gpx_path.exists():
    with open(gpx_path, 'r', encoding='utf-8') as f:
        gpx = gpxpy.parse(f)
    
    print('Waypoints encontrados no GPX lna_assis.gpx:')
    for i, waypoint in enumerate(gpx.waypoints[:10]):  # Mostra só os primeiros 10
        print(f'{i+1:2d}. Nome: "{waypoint.name}" | Lat: {waypoint.latitude:.6f} | Lon: {waypoint.longitude:.6f}')
    
    print(f'\nTotal de waypoints: {len(gpx.waypoints)}')
    
    # Procura por torres que contenham '75'
    torres_75 = [wp for wp in gpx.waypoints if '75' in str(wp.name)]
    if torres_75:
        print(f'\nTorres com "75" no nome:')
        for wp in torres_75:
            print(f'  Nome: "{wp.name}"')
    else:
        print('\nNenhuma torre com "75" encontrada')
        
    # Mostra também os últimos 10 waypoints
    print('\nÚltimos 10 waypoints:')
    for i, waypoint in enumerate(gpx.waypoints[-10:]):
        idx = len(gpx.waypoints) - 10 + i + 1
        print(f'{idx:2d}. Nome: "{waypoint.name}" | Lat: {waypoint.latitude:.6f} | Lon: {waypoint.longitude:.6f}')
else:
    print('Arquivo lna_assis.gpx não encontrado')
