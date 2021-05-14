
const readline = require('readline')
const { ConnectionBuilder } = require('electron-cgi')

const connection = new ConnectionBuilder()
    .connectTo('dotnet', 'run', '--project', '../c-sharp')
    .build();

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});


connection.onDisconnect = () => {
    console.log('Lost connection to the .Net process');
};


function greetUser(greeting) {
    connection.send('greeting', greeting, (res) => {
        console.log(res)
    })
}



rl.question('What is your name bro?: ', (answer) => {
    greetUser(answer);

    connection.on('emis-ping', (res) => {
        console.log(res)
    })

})


// greetUser('Kenny');


