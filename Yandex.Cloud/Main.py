from Src.Services.Repo import repo
from Src.Services.Helper import helper

import connexion
import json


repo  =  repo.create(is_demo=True)
options = {"swagger_ui": True}
api = connexion.FlaskApp(__name__, specification_dir='./', options=options)

# =======================================================
# Акты

@api.route("/api/acts/<uid>", methods=['GET'])
def getAct(uid):
    """
    Получить карточку акта
    """
    items = list(filter(lambda x : x.uid == uid, repo.get_acts())) 
    if len(items) > 0:
        # Возвращает 200
        return items[0].toJSON()
    else:
        # Возвращаем 204
        return 'Not Found', 204, {'x-error': 'not found'}



@api.route("/api/acts", methods=['GET'])
def getActs():
    """
    Получить список всех актов
    """
    items = repo.get_acts()
    result = []
    for item in items:
        result.append(helper.toDict(item))

    # Возвращает 200
    result = json.dumps(result, sort_keys = True, indent = 4)  
    return '{"acts":' + result + '}'


# =======================================================
# Застройщики

@api.route("/api/contractors/<uid>", methods=['GET'])
def getContracts(uid):
    """
    Получить карточку застройщика
    """
    items = list(filter(lambda x : x.uid == uid, repo.get_contractors())) 
    if len(items) > 0:
        # Возвращает 200
        return items[0].toJSON()
    else:
        # Возвращаем 204
        return 'Not Found', 204, {'x-error': 'not found'}



@api.route("/api/contractors", methods=['GET'])
def getContract():
    """
    Получить список всех застройщиков
    """
    items = repo.get_contractors()
    result = []
    for item in items:
        result.append(helper.toDict(item))

    # Возвращает 200
    result = json.dumps(result, sort_keys = True, indent = 4)  
    return '{"contractors":' + result + '}'


# =======================================================
# Исполнители

@api.route("/api/executors/<uid>", methods=['GET'])
def getExecutors(uid):
    """
    Получить карточку исполнителя
    """
    items = list(filter(lambda x : x.uid == uid, repo.get_executors())) 
    if len(items) > 0:
        # Возвращает 200
        return items[0].toJSON()
    else:
        # Возвращаем 204
        return 'Not Found', 204, {'x-error': 'not found'}



@api.route("/api/executors", methods=['GET'])
def getExecutor():
    """
    Получить список всех исполнителей
    """
    items = repo.get_executors()
    result = []
    for item in items:
        result.append(helper.toDict(item))

    # Возвращает 200
    result = json.dumps(result, sort_keys = True, indent = 4)  
    return '{"executors":' + result + '}'


# =======================================================
# Объекты капитального строительства

@api.route("/api/building/<uid>", methods=['GET'])
def getBuildings(uid):
    """
    Получить карточку ОКС
    """
    items = list(filter(lambda x : x.uid == uid, repo.get_buildings())) 
    if len(items) > 0:
        # Возвращает 200
        return items[0].toJSON()
    else:
        # Возвращаем 204
        return 'Not Found', 204, {'x-error': 'not found'}



@api.route("/api/building", methods=['GET'])
def getBuilding():
    """
    Получить список всех ОКС
    """
    items = repo.get_buildings()
    result = []
    for item in items:
        result.append(helper.toDict(item))

    # Возвращает 200
    result = json.dumps(result, sort_keys = True, indent = 4)  
    return '{"building":' + result + '}'



if __name__ == '__main__':
    api.add_api("Swagger.yaml")
    api.run(debug=True, port=8080)
    


