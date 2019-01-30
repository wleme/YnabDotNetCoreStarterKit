# YNAB starter kit in .net core 2.2

Want to start developing custom Ynab tools using .net core ? You can use this project as a base

This project implements the OAuth middleware and saves the tokens in a cookie. It then retrieves the access token every time it pulls data from their API. It also leverages the new HttpClientFactory 
(introduced on .net core 2.1).

You need to get your own Client Id and Client Secret and update appsetings.json