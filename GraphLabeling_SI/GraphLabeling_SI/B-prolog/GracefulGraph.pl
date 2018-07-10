
% Just return first possibility or false if cant be graceful.
go(Graph,M,N) :-
       
 graceful_graph(Graph, M,N,Nodes,Edges),
        
 write(nodes:Nodes),
 nl,
        
 write(edges:Edges),nl.



% Just count the number of solutions
 go2(Graph,M,N) :-
                       
 findall(_, graceful_graph(Graph, M,N,_Nodes,_Edges),L),
        
 length(L, Len),
        
 format("Number of solutions: ~d\n",[Len]).
 
     
% Main function  


graceful_graph(Graph, M,N,Nodes,Edges) :-

        
 length(Nodes,N),
        
 Nodes :: 0..M,

        
 length(Edges,M),
        
 Edges :: 1..M,

        
 alldifferent(Edges),
        
 alldifferent(Nodes),

      
 foreach(([From,To],Edge) in (Graph,Edges),
 [FromN,ToN],
               
	 (
          
	 element(From,Nodes,FromN),
                    
	 element(To,Nodes,ToN),
                    
	 abs(FromN - ToN) #= Edge
                
	 )
        
        ),

        
 term_variables([Nodes,Edges],Vars),
        
 labeling(Vars).



