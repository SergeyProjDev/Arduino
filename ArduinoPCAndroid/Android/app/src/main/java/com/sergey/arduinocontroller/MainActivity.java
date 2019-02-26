package com.sergey.arduinocontroller;

import android.os.AsyncTask;
import android.os.Environment;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ip = findViewById(R.id.ip);
        hint = findViewById(R.id.textView);

        light = findViewById(R.id.btnLight);
    }

    EditText ip;
    public static TextView hint;
    Button light;



    boolean flag = true;
    MessageSender ms;
    public void Light(View v){ //on click
        ms = new MessageSender();
        if (flag){
            ms.execute(ip.getText().toString(), "1");
            light.setBackgroundResource(R.drawable.on);
            flag = !flag;
        }else {
            ms.execute(ip.getText().toString(), "2");
            light.setBackgroundResource(R.drawable.off);
            flag = !flag;
        }


    }
}

class MessageSender extends AsyncTask<String, Void, Void> {

    Socket s;
    PrintWriter pw;

    String data;

    @Override
    protected Void doInBackground(String... voids) {
        try {
            String ip = voids[0];
            String msg = voids[1];

            //s = new Socket(ip, 8080);
            //pw = new PrintWriter(s.getOutputStream());
            //pw.write(msg);
            //pw.flush();
            //pw.close();
            //s.close();

            s = new Socket();
            s.connect(new InetSocketAddress(ip,1234),5000);

            s.setSoTimeout(10);
            pw = new PrintWriter(s.getOutputStream());

            pw.write(msg);
            pw.flush();



            data = "Command sent!";
        }catch (Exception e){
            data = e.toString();
        }
        publishProgress();
        return null;
    }

    protected void onPostExecute(Void result){
        MainActivity.hint.setText("Info: " + data);
        data = "";
    }

}
