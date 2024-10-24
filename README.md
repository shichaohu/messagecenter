### 1、场景概述
消息中心适用于软件系统有发送消息的场景，其中消息渠道目前支持邮件、短息（发送逻辑待实现）、企业微信（发送逻辑待实现）
### 2、技术框架
- .net 8
- mysql 9.0.1
- RabbitMQ 4.0.0
  - http://139.155.133.198:15672/ admin/admin123456
### 2、系统运行时序图
![alt text](消息中心系统运行时序图.jpg)
### 3、接口文档
- 向消息中心发送消息
```
post http://127.0.0.1:9082/api/Message/SendMessage
[
  {
    "Title": "合同到期提醒标题",
    "Content": "合同到期提醒内容",
    "DynamicParameter":{
      "UserName": " Mr. John Doe",
      "ContractNo": "ht-001"
    },
    "LinkUrl": "www.baidu.com",
    "LinkText": "点击此处处理",
    "BusinessTypeKey": "ContractExpirationReminder",
    "BusinessTypeValue": "合同到期提醒",
    "Sendchannel": "0",
    "Sender": {
      "UserId": "1001",
      "UserName": "sch"
    },
    "Receiver": [
      {
        "ReceiverUserid": "2001",
        "ReceiverName": "shichaohu",
        "Email": "shichaohu@live.com",
        "CcEmail": "502242999@qq.com",
        "Phone": "13188888888",
        "EnterpriseWechat": "ty-xx001",
        "Dingtalk": "dd-xx002",
        "OtherReceiveChannel": ""
      }
    ]
  }
]
```