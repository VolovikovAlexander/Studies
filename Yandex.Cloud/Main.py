from Src.Services.Repo import repo
from Src.Services.Helper import helper

import connexion
import json


repo  =  repo.create(is_demo=True)
api = connexion.App(__name__, specification_dir='./')


@api.route("/api/acts/<int:uid>")
def getAct(uid):
    """
    Получить карточку акта
    """
    object = filter(lambda x : x.uid == uid, repo.acts )
    if object is not None:
        # Возвращает 200
        return object.toJSON()
    else:
        # Возвращаем 204
        return 'Not Found', 204, {'x-error': 'not found'}



@api.route("/api/acts")
def getActs():
    """
    Получить список всех актов
    """
    items = repo.get_acts()
    if not any(items):
        # Возвращаем 204
        return 'Not Found', 204, {'x-error': 'not found'}
    else:
        result = []
        for item in items:
            result.append(helper.toDict(item))

        # Возвращает 200
        result = json.dumps(result, sort_keys = True, indent = 4)  
        return '{"acts":' + result + '}'


if __name__ == '__main__':
    api.run(port=8080)


