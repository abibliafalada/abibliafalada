---
layout: home
title: A Biblia Falada
---

{% for post in site.posts %}
##  [{{ post.title }}]({{ post.url | prepend: site.baseurl }})
  {{ post.date | date_to_string }}
  {{ post.content }}
{% endfor %}
