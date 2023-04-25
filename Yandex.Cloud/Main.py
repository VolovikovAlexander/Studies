from Src.Services.Repo import repo
import connexion
import json


repo  =  repo.create()
api = connexion.App(__name__, specification_dir='./')


@api.route("/api/acts/<int:uid>")
def getAct(uid):
    """
    Получить карточку акта
    """
    item = filter(lambda x : x.uid == uid, repo.acts )
    return json.dumps(item)


@api.route("/api/acts")
def getActs():
    """
    Получить список всех актов
    """
    result = []
    for x in repo.get_acts():
        result.append(x.toJSON())

    json_result = str(result).replace( "'","")
    return '{"acts":' + json_result + '}'


if __name__ == '__main__':
    api.run(port=8080)


