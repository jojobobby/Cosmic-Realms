﻿using System;
namespace wServer.networking.packets
{
    public enum PacketId : byte
    {
        FAILURE = 0,
        CREATE_SUCCESS = 81,
        CREATE = 12,
        PLAYERSHOOT = 66,
        MOVE = 16,
        PLAYERTEXT = 47,
        TEXT = 96,
        SERVERPLAYERSHOOT = 92,
        DAMAGE = 97,
        UPDATE = 42,
        UPDATEACK = 91,
        NOTIFICATION = 33,
        NEWTICK = 68,
        INVSWAP = 25,
        USEITEM = 1,
        SHOWEFFECT = 38,
        HELLO = 183,
        GOTO = 9,
        INVDROP = 18,
        INVRESULT = 63,
        RECONNECT = 14,
        PING = 85,
        PONG = 64,
        MAPINFO = 74,
        LOAD = 26,
        PIC = 46,
        SETCONDITION = 60,
        TELEPORT = 45,
        USEPORTAL = 6,
        DEATH = 83,
        BUY = 93,
        BUYRESULT = 50,
        AOE = 89,
        GROUNDDAMAGE = 98,
        PLAYERHIT = 10,
        ENEMYHIT = 19,
        AOEACK = 77,
        SHOOTACK = 35,
        OTHERHIT = 57,
        SQUAREHIT = 13,
        GOTOACK = 79,
        EDITACCOUNTLIST = 62,
        ACCOUNTLIST = 44,
        QUESTOBJID = 28,
        CHOOSENAME = 23,
        NAMERESULT = 22,
        CREATEGUILD = 95,
        GUILDRESULT = 82,
        GUILDREMOVE = 49,
        GUILDINVITE = 41,
        ALLYSHOOT = 36,
        ENEMYSHOOT = 52,
        REQUESTTRADE = 34,
        TRADEREQUESTED = 80,
        TRADESTART = 31,
        CHANGETRADE = 55,
        TRADECHANGED = 4,
        ACCEPTTRADE = 3,
        CANCELTRADE = 39,
        TRADEDONE = 94,
        TRADEACCEPTED = 78,
        CLIENTSTAT = 75,
        CHECKCREDITS = 20,
        ESCAPE = 87,
        FILE = 56,
        INVITEDTOGUILD = 58,
        JOINGUILD = 27,
        CHANGEGUILDRANK = 11,
        PLAYSOUND = 59,
        GLOBAL_NOTIFICATION = 24,
        RESKIN = 15,
        PETUPGRADEREQUEST = 67,
        ACTIVE_PET_UPDATE_REQUEST = 90,
        ACTIVEPETUPDATE = 76,
        NEW_ABILITY = 21,
        PETYARDUPDATE = 53,
        EVOLVE_PET = 7,
        DELETE_PET = 8,
        HATCH_PET = 86,
        ENTER_ARENA = 48,
        IMMINENT_ARENA_WAVE = 5,
        ARENA_DEATH = 17,
        ACCEPT_ARENA_DEATH = 84,
        VERIFY_EMAIL = 61,
        RESKIN_UNLOCK = 40,
        PASSWORD_PROMPT = 69,
        QUEST_FETCH_ASK = 51,
        QUEST_REDEEM = 37,
        QUEST_FETCH_RESPONSE = 65,
        QUEST_REDEEM_RESPONSE = 88,
        SERVER_FULL = 110,
        QUEUE_PING = 111,
        QUEUE_PONG = 112,
        MARKET_COMMAND = 99,
        PET_CHANGE_FORM_MSG = 150,
        QUEST_ROOM_MSG = 155,
        KEY_INFO_REQUEST = 151,
        KEY_INFO_RESPONSE = 152,
        MARKET_RESULT = 100,
        SET_FOCUS = 108,
        SWITCH_MUSIC = 106,
        CLAIM_LOGIN_REWARD_MSG = 153,
        LOGIN_REWARD_MSG = 154,
        ACHIEVEMENT_UNLOCKED = 156,
        FORGE_ITEM = 157,
        CRITICALDAMAGE = 158,
        SWORDDMG = 159,
        MARKET_SEARCH = 171,
        MARKET_SEARCH_RESULT = 172,
        MARKET_BUY = 173,
        MARKET_BUY_RESULT = 174,
        MARKET_ADD = 175,
        MARKET_ADD_RESULT = 176,
        MARKET_REMOVE = 177,
        MARKET_REMOVE_RESULT = 178,
        MARKET_MY_OFFERS = 179,
        MARKET_MY_OFFERS_RESULT = 180,
        FORGE_LIST = 181,
        FORGE_LIST_RESULT = 182,
        //DONT USE 183
        POTIONS = 184,
        UPGRADE = 185,
        WeatherProperties = 186
    }
}
