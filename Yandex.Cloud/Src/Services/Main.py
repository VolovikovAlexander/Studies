from flask import Flask, request
import json
from Src.Services.Reposity import reposity

main_reposity  = reposity()

# Получить по коду конкретный акт
@app.route("/api/acts/<int:uid>", methods=["GET"])
def getAct(uid):
    item = filter(lambda x : x.uid == uid, main_reposity.acts() )
    return json.dumps(item)

# Получить реестр актов
@app.route('/api/acts', methods=['GET'])
def getActs():
    json.dump(main_reposity.acts())



