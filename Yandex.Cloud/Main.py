from Src.Services.Repo import repo
from Src.Services.Helper import helper

import connexion
import json


repo  =  repo.create(is_demo=True)
api = connexion.App(__name__, specification_dir='./')

# =======================================================
# Акты

@api.route("/api/acts/<uid>")
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



@api.route("/api/acts")
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

@api.route("/api/contractors/<uid>")
def getAct(uid):
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



@api.route("/api/contractors")
def getActs():
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

@api.route("/api/executors/<uid>")
def getAct(uid):
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



@api.route("/api/executors")
def getActs():
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

@api.route("/api/building/<uid>")
def getAct(uid):
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



@api.route("/api/building")
def getActs():
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
    api.run(port=8080)


